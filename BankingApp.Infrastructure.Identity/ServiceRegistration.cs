using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Infrastructure.Identity.Contexts;
using BankingApp.Infrastructure.Identity.Entities;
using BankingApp.Infrastructure.Identity.Repository;
using BankingApp.Infrastructure.Identity.Seeds;
using BankingApp.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            #region context
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                service.AddDbContext<BankingAppIdentityDbContext>(option => option.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                service.AddDbContext<BankingAppIdentityDbContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("DefaultIdentityConnection"),
                m => m.MigrationsAssembly(typeof(BankingAppIdentityDbContext).Assembly.FullName)));
            }
            #endregion

            #region Identity
            service.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BankingAppIdentityDbContext>()
                .AddDefaultTokenProviders();
            #endregion

            service.AddTransient<IUserRepository, UserRepository>();
            #region Services
            service.AddTransient<IAccountService, AccountServices>();
            service.AddAuthentication();
            #endregion

            #region cookie
            service.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Login";
                option.AccessDeniedPath = "/Login/AccessDenied";
            });
            #endregion
        }

        public static async Task RunIdentitySeeds(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;

            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                await DafaultRoles.SeedAsync(roleManager);
                await DefualtClientUser.SeedAsync(userManager, roleManager);
                await DefaultSuperAdminUser.SeedAsync(userManager, roleManager);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
