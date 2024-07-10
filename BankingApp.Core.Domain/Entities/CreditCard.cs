using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class CreditCard: Product
    {
        public double CreditLimit {  get; set; }

        public byte CutoffDay { get; set; }

        public byte PaymentDay { get; set; }

    }
}
