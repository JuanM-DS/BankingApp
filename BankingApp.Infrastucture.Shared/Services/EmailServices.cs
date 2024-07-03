using BankingApp.Core.Application.DTOs.Email;
using BankingApp.Core.Application.Interfaces.Services;

namespace BankingApp.Infrastructure.Shared.Services
{
    public class EmailServices : IEmailServices
    {
        public Task SendEmailAsync(EmailRequestDTO request)
        {
        }
    }
}
