using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWeb.Application.Interfaces;
using NewsWeb.WebApp.Models;

namespace NewsWeb.WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                TempData["ReturnUrl"] = ReturnUrl;
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginModel userLoginModel)
        {
            string returnUrl = TempData["ReturnUrl"]?.ToString() ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserManager.FindByEmailAsync(userLoginModel.Email);
                if (user != null)
                {
                    await _unitOfWork.SignInManager.SignOutAsync();

                    var result = await _unitOfWork.SignInManager.PasswordSignInAsync(user, userLoginModel.Password, userLoginModel.IsRememberMe, true);

                    if (result.Succeeded)
                    {
                        await _unitOfWork.UserManager.ResetAccessFailedCountAsync(user);
                        await _unitOfWork.UserManager.SetLockoutEndDateAsync(user, null);

                        return Redirect(returnUrl);
                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutEndUtc = await _unitOfWork.UserManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockoutEndUtc.Value - DateTime.UtcNow;
                        ModelState.AddModelError(string.Empty, $"This account has been locked out, please try again {timeLeft.Minutes} minutes later.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid e-mail or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid e-mail or password.");
                }
            }
            return View(userLoginModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _unitOfWork.SignInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
