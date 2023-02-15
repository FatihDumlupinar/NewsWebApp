using NewsWeb.Models.Mail;

namespace NewsWeb.Infrastructure.Services.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken = default);
    }
}
