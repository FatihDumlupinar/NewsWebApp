using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWeb.Application.Interfaces;
using NewsWeb.Infrastructure.Services.RabbitMQ;
using NewsWeb.Models.Mail;
using NewsWeb.WebApp.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Security.Claims;
using System.Text;

namespace NewsWeb.WebApp.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        #region Ctor&Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRabbitMqService _rabbitMqService;

        public IssueController(IUnitOfWork unitOfWork, IRabbitMqService rabbitMqService)
        {
            _unitOfWork = unitOfWork;
            _rabbitMqService = rabbitMqService;
        }

        #endregion

        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var issuesList = await _unitOfWork.Issues.FindAsync(i => i.IsActive, cancellationToken);

            var listModel = issuesList.OrderByDescending(c => c.CreateDate).Select(i => new IssueListModel()
            {
                Id = i.Id,
                CreateDate = i.CreateDate,
                Title = i.Title,

            }).ToList();

            return View(listModel);
        }

        public async Task<ActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var issue = await _unitOfWork.Issues.GetByIdAsync(id, cancellationToken);

            var detailModel = new IssueDetailModel()
            {
                CreateDate = issue.CreateDate,
                Description = issue.Description,
                Id = issue.Id,
                Title = issue.Title,
                UpdateDate = issue.UpdateDate,
            };

            return View(detailModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IssueCreateModel createModel, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.Issues.AddAsync(new()
                {
                    Description = createModel.Description,
                    Title = createModel.Title,
                }, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);

                await _unitOfWork.Issues.RefreshCacheAsync(cancellationToken);

                using var connection = _rabbitMqService.CreateChannel();
                using var model = connection.CreateModel();
                
                //gmail,outlook hesaplarında 3. parti uygulamalara izin vermiyor o yüzden eposta adresi kaydetmedim

                var currentUserEmail = _unitOfWork.HttpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new MailRequest()
                {
                    Body = "Issue created",
                    Subject = "Issue",
                    ToEmail = currentUserEmail
                }));
                model.BasicPublish("UserExchange",
                                     string.Empty,
                                     basicProperties: null,
                                     body: body);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

    }
}
