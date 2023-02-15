using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NewsWeb.Models.Mail;

namespace NewsWeb.Infrastructure.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken = default)
        {
            using var smtp = new SmtpClient();

            try
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();

                builder.HtmlBody = mailRequest.Body;

                email.Body = builder.ToMessageBody();

                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                await smtp.SendAsync(email, cancellationToken);

                _logger.LogInformation("Email sended.", email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Email send error.", ex.Message, ex);
            }
            finally
            {
                if (smtp.IsConnected)
                {
                    smtp.Disconnect(true);
                }

                smtp.Dispose();
            }

        }
    }
}
