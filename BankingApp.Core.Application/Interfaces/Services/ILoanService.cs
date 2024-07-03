using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface ILoanService : IGenericService<SaveLoanViewModel, LoanViewModel, Loan>
    {

    }
}
