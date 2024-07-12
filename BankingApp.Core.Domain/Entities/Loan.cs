using BankingApp.Core.Domain.Common;

namespace BankingApp.Core.Domain.Entities
{
    public class Loan: BaseEntity
    {
        public double Principal { get; set; }
        public double Balance { get; set; }
        public double InterestRate { get; set; }
        public int Term { get; set; }
        public double Installment {  get; set; }
        public byte PaymentDay { get; set; }

    }
}
