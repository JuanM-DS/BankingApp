using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Domain.Common;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Infrastructure.Persistence.Seeds
{
    public static class DefaultClientPrincipalAccount
    {
        public static async Task SeedAsync(ISavingsAccountRepository savingsAccountRepository, IProductRepository productRepository)
        {
            var principalAccount = new SavingsAccount()
            {
                Balance = 10000,
                IsPrincipal = true,
                UserName = "ClientUser",
                CreatedTime = DateTime.Now,
                CreatedBy = "System",
            };

            var product = new Product()
            {
                Id = 100100099,
                UserName = "ClientUser",
                CreatedTime = DateTime.Now,
                CreatedBy = "System",
                Type = (byte)ProductTypes.SavingsAccount
            };

            var existsAccount = await savingsAccountRepository.GetByIdAsync(100100099);

            if (existsAccount is not null) return;

            await savingsAccountRepository.AddAsync(principalAccount);
            await productRepository.AddAsync(product);
        }
    }
}
