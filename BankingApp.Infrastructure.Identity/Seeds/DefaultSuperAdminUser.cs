﻿using BankingApp.Core.Application.Enums;
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
                CreatedTime = DateTime.Now,
                CreatedBy = "System",
                IdCard = "000-0000000-0",
            };

            var userByUserName = await userManager.FindByNameAsync(user.UserName);
            if (userByUserName is not null) return;

            var userByEmail = await userManager.FindByEmailAsync(user.Email);
            if (userByEmail is not null) return;


            await userManager.CreateAsync(user, "123Pa$$word!");

            await userManager.AddToRolesAsync(user, [RoleTypes.Admin.ToString(), RoleTypes.SuperAdmin.ToString(), RoleTypes.Client.ToString()]);
        }
    }
}
