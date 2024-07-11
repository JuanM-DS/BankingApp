using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface ICreditCardRepository : IGenericRepository<CreditCard>
    {
        Task<CreditCard> AddAsync(CreditCard creditCard);
    }
}
