using BankingApp.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Infrastructure.Identity.Seeds
{
    public static class DafaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            var clientExists = await roleManager.RoleExistsAsync(RoleTypes.Client.ToString());
            var superAdminExists = await roleManager.RoleExistsAsync(RoleTypes.SuperAdmin.ToString());
            var adminExists = await roleManager.RoleExistsAsync(RoleTypes.Admin.ToString());

            if (!clientExists)
                await roleManager.CreateAsync(new IdentityRole(RoleTypes.Client.ToString()));

            if(!adminExists)
                await roleManager.CreateAsync(new IdentityRole(RoleTypes.Admin.ToString()));

            if(!superAdminExists)
                await roleManager.CreateAsync(new IdentityRole(RoleTypes.SuperAdmin.ToString()));
        }
    }
}
