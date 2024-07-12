using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface ISavingsAccountRepository : IGenericRepository<SavingsAccount>
    {
        Task<SavingsAccount> AddAsync(SavingsAccount savingsAccount);
        Task<SavingsAccount> GetPrincipalAccountAsync(string userName);
    }
}
