using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface IBeneficiaryRepository : IGenericRepository<Beneficiary>
    {
        Task<Beneficiary> GetBeneficiary(string userName, int AccountNumber);
    }
}
