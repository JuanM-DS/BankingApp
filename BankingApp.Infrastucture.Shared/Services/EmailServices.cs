using BankingApp.Core.Application.DTOs.Email;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BankingApp.Infrastructure.Shared.Services
{
    public class EmailServices(IOptions<EmailSettings> emailSettings) : IEmailServices
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;

        public async Task SendEmailAsync(EmailRequestDTO request)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.EmailFrom);
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            var body = new BodyBuilder()
            {
                HtmlBody = request.Body
            };
            email.Body = body.ToMessageBody();

            try
            {
                using var sender = new SmtpClient();
                sender.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await sender.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await sender.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPassword);
                await sender.SendAsync(email);
                await sender.DisconnectAsync(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
