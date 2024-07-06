using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IPaymentService : IGenericService<SavePaymentViewModel, PaymentViewModel, Payment>
    {
        Task<double> TransactionsTillCutoffDay(int CutoffDay, int CreditCardNumber);

        Task<List<PaymentViewModel>> GetAllViewModel(PaymentQueryFilters? filters = null);

        Task<List<PaymentViewModel>> GetAllTransactions();

        Task<List<PaymentViewModel>> GetAllTransfersOfToday(DateTime time);
    }
}
