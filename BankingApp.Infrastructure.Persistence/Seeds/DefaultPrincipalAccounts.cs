using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Infrastructure.Persistence.Seeds
{
    public static class DefaultPrincipalAccounts
    {
        public static async Task SeedAsync(ISavingsAccountRepository savingsAccountRepository)
        {
            var defaultClientPrincipalAccount = new SavingsAccount()
            {
                Balance = 10000,
                IsPrincipal = true,
                UserName = "ClientUser",
                CreatedTime = DateTime.Now,
                CreatedBy = "System",
            };

            var defaultSuperAdminPrincipalAccount = new SavingsAccount()
            {
                Balance = 10000,
                IsPrincipal = true,
                UserName = "SuperAdminUser",
                CreatedTime = DateTime.Now,
                CreatedBy = "System",
            };

            var existAccounts = await savingsAccountRepository.GetByIdAsync(100100098);

            if (existAccounts is not null) return;

            await savingsAccountRepository.AddAsync(defaultClientPrincipalAccount);
            await savingsAccountRepository.AddAsync(defaultSuperAdminPrincipalAccount);
        }
    }
}
