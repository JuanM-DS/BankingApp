using BankingApp.Core.Application.DTOs.Email;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(EmailRequestDTO request);
    }
}
