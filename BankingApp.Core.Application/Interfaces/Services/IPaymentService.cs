using BankingApp.Core.Application.CustomEntities;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IPaymentService : IGenericService<SavePaymentViewModel, PaymentViewModel, Payment>
    {
        Task<double> TransactionsTillCutoffDay(int CutoffDay, int CreditCardNumber);

        Task<List<PaymentViewModel>> GetAllViewModel(PaymentQueryFilters? filters = null);

        Task<List<PaymentViewModel>> GetAllTransactions(List<PaymentTypes> paymentTypes);

        Task<List<PaymentViewModel>> GetAllTransfersOfToday(DateTime time);
        Task<Response<SaveExpressPaymentViewModel>> ExpressPayment(SaveExpressPaymentViewModel vm, string UserName);
        Task ConfirmTransactionExpressPost(SaveExpressPaymentViewModel vm, string UserName);
        Task<Response<SaveCreditCardPaymentViewModel>> CreditCardPayment(SaveCreditCardPaymentViewModel vm, string UserName);
        Task<Response<SaveLoanPaymentViewModel>> LoanPayment(SaveLoanPaymentViewModel vm, string UserName);
        Task<Response<SavePaymentToBeneficiariesViewModel>> PaymentToBeneficiaries(SavePaymentToBeneficiariesViewModel vm, string UserName);
        Task<Response<SaveCashAdvancesViewModel>> CashAdvances(SaveCashAdvancesViewModel vm, string UserName);
        Task<Response<SaveTransferBetweenAccountsViewModel>> TransferBetweenAccounts(SaveTransferBetweenAccountsViewModel vm, string UserName);
        Task ConfirmTransactionBeneficiaryPost(SavePaymentToBeneficiariesViewModel vm, string UserName);

        Task Disbursement(SaveLoanViewModel loan);
        Task SavingsAccountDeletion(SaveSavingsAccountViewModel accountToDelete);
    }
}
