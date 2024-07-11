using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface ILoanRepository : IGenericRepository<Loan>
    {
        Task<Loan> AddAsync(Loan loan);
    }
}
