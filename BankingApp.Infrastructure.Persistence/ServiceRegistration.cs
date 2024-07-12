using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Infrastructure.Persistence.Contexts;
using BankingApp.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BankingApp.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddTransient<ICreditCardRepository, CreditCardRepository>();
            services.AddTransient<ILoanRepository, LoanRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<ISavingsAccountRepository, SavingsAccountRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            #endregion
        }
    }
}
