using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IBeneficiaryService, BeneficiaryService>();
            services.AddTransient<ICreditCardService, CreditCardService>();
            services.AddTransient<ILoanService, LoanService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<ISavingsAccountService, SavingsAccountService>();
            #endregion
        }
    }
}
