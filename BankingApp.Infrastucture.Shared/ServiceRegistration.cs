using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Domain.Settings;
using BankingApp.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            #region Services
            service.AddTransient<IEmailServices, EmailServices>();
            #endregion

            #region Settings
            service.Configure<EmailSettings>(option => configuration.GetSection("EmailSettings").Bind(option));
            #endregion
        }
    }
}
