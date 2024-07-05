using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Domain.Settings;
using BankingApp.Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            #region Services
            service.AddTransient<IEmailService, EmailService>();
            service.AddSingleton<IUrlService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var baseUrl = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UrlService(baseUrl);
            });
            #endregion

            #region Settings
            service.Configure<EmailSettings>(option => configuration.GetSection("EmailSettings").Bind(option));
            #endregion
        }
    }
}
