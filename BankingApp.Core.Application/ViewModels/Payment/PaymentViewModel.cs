using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Application.ViewModels.User;

namespace BankingApp.Core.Application.ViewModels.Payment
{
    public class PaymentViewModel
    {
        public Guid Id { get; set; }

        public int? FromProductId { get; set; }

        public int? ToProductId { get; set; }

        public double Amount { get; set; }
        public DateTime CreatedTime { get; set; }

        public byte Type { get; set; }

        public byte ProductType { get; set; }

        public string UserName { get; set; }


        ////Navigation Property
        //public SavingsAccountViewModel? FromAccount { get; set; }
        //public CreditCardViewModel? FromCrediCard { get; set; }
        //public LoanViewModel? FromLoan { get; set; }

        //public SavingsAccountViewModel? ToAccount { get; set; }

        //public CreditCardViewModel? ToCreditCard { get; set; }
        //public LoanViewModel? ToLoan { get; set; }

        public UserViewModel User { get; set; }
    }
}
