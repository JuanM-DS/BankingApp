using BankingApp.Core.Application.Enums;
using BankingApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Infrastructure.Identity.Seeds
{
    public static class DefualtClientUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user  = new ApplicationUser()
            {
                FirstName = "Jone",
                LastName = "Dou",
                UserName = "ClientUser",
                Email = "ClientUser@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var userByUserName = await userManager.FindByNameAsync(user.UserName);
            if (userByUserName is null) return;

            var userByEmail = await userManager.FindByEmailAsync(user.Email);
            if (userByEmail is null) return;

            await userManager.CreateAsync(user);

            await userManager.AddToRoleAsync(user, RoleTypes.Client.ToString());
        }
    }
}
