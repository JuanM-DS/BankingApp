using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int? FromAccountId { get; set; }

        public int? ToAccountId { get; set; }

        public double Amount { get; set; }

        public string Type { get; set; }

        public int? ToCreditCardId { get; set; }

        public int? ToLoanId { get; set; }


        //Navigation Property
        public SavingsAccount? FromAccount { get; set; }

        public SavingsAccount? ToAccount { get; set; }

        public CreditCard? ToCreditCard { get; set; }

        public Loan? ToLoan { get; set; }
    }
}
