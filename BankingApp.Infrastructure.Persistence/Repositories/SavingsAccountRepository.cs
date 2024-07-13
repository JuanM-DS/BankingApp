using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Domain.Entities;
using BankingApp.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class SavingsAccountRepository : GenericRepository<SavingsAccount>, ISavingsAccountRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly IProductRepository _productRepository;

        public SavingsAccountRepository(ApplicationContext dbContext, IProductRepository productRepository) : base(dbContext)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
        }

        public override async Task<SavingsAccount> AddAsync(SavingsAccount savingsAccount)
        {
            await base.AddAsync(savingsAccount);
            await _dbContext.SaveChangesAsync();
            return savingsAccount;
        }

        public async Task<SavingsAccount> GetPrincipalAccountAsync(string userName)
        {
            var savingsAccount = await _dbContext.SavingsAccounts.FirstOrDefaultAsync(ac => ac.UserName == userName && ac.IsPrincipal == true);
            return savingsAccount;
        }
        public override async Task UpdateAsync(SavingsAccount entity, int id)
        {
            var entry = await _dbContext.Set<SavingsAccount>().FindAsync(id);
            entry.Balance = entity.Balance;
            await base.UpdateAsync(entry, id);
        }
    }
}