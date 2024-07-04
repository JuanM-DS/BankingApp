using BankingApp.Core.Application.Enums;
using BankingApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Infrastructure.Identity.Seeds
{
    public  static class DefaultSuperAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user = new ApplicationUser()
            {
                FirstName = "Jhone",
                LastName = "Dou",
                UserName = "SuperAdminUser",
                Email = "SuperAdminUser@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var userByUserName = await userManager.FindByNameAsync(user.UserName);
            if (userByUserName is null) return;

            var userByEmail = await userManager.FindByEmailAsync(user.Email);
            if (userByEmail is null) return;

            await userManager.CreateAsync(user);

            await userManager.AddToRolesAsync(user, [RoleTypes.Admin.ToString(), RoleTypes.SuperAdmin.ToString(), RoleTypes.Client.ToString()]);
        }
    }
}
