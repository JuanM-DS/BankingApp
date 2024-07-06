using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int? FromProductId { get; set; }

        public int? ToProductId { get; set; }

        public double Amount { get; set; }

        public byte Type { get; set; }

        //Navigation Property
        public SavingsAccount? FromAccount { get; set; }
        public CreditCard? FromCrediCard { get; set; }
        public Loan? FromLoan { get; set; }

        public SavingsAccount? ToAccount { get; set; }

        public CreditCard? ToCreditCard { get; set; }
        public Loan? ToLoan { get; set; }
    }
}
