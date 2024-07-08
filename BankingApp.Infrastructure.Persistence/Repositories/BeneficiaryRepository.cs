using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Helpers;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Domain.Entities;
using BankingApp.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        private readonly ApplicationContext _dbContext;

        public BeneficiaryRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task<Beneficiary> GetBeneficiary (int AccountNumber)
        {
            Beneficiary beneficiary = _dbContext.Set<Beneficiary>().FirstOrDefault(b => b.UserName == User.UserName && b.AccountNumber == AccountNumber);

            return beneficiary;
        }
    }
}
