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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationUserDTO User;

        public BeneficiaryRepository(ApplicationContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            User = _httpContextAccessor.HttpContext.Session.Get<ApplicationUserDTO>("user");
        }

        public async Task<Beneficiary> GetBeneficiary (int AccountNumber)
        {
            Beneficiary beneficiary = _dbContext.Set<Beneficiary>().FirstOrDefault(b => b.UserName == User.UserName && b.AccountNumber == AccountNumber);

            return beneficiary;
        }
    }
}
