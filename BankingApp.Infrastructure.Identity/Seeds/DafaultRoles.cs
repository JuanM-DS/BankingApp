using BankingApp.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Infrastructure.Identity.Seeds
{
    public static class DafaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(RoleTypes.Client.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleTypes.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleTypes.SuperAdmin.ToString()));
        }
    }
}
