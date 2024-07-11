using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Domain.Entities;
using BankingApp.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationContext _dbContext;

        public LoanRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<Loan> AddAsync(Loan loan)
        {
            await base.AddAsync(loan);
            await _dbContext.SaveChangesAsync();
            return loan;
        }
        public override async Task UpdateAsync(Loan entity, int id)
        {
            var entry = await _dbContext.Set<Loan>().FindAsync(id);
            entry.Balance = entity.Balance;
            await base.UpdateAsync(entry, id);
        }
    }
}
