using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Domain.Entities;
using BankingApp.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BeneficiaryRepository(ApplicationContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Beneficiary> GetBeneficiary (string Username, int AccountNumber)
        {
            Beneficiary beneficiary = _dbContext.Set<Beneficiary>().FirstOrDefault(b => b.UserName == Username && b.AccountNumber == AccountNumber);

            return beneficiary;
        }
    }
}
