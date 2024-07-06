using BankingApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Double> TransactionsTillCutoffDay(int CutoffDay, int CreditCardNumber);
    }
}
