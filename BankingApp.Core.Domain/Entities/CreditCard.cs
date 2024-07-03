using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class CreditCard: BaseEntity
    {
        public double Balance {  get; set; }

        public double CreditLimit {  get; set; }

        public byte CutoffDay { get; set; }

        public byte PaymentDay { get; set; }

        //Navigation Property
        public ICollection<Payment> Payments { get; set; }
    }
}
