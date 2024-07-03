using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class SavingsAccount: BaseEntity
    {
        public double Balance { get; set; }

        public bool IsPrincipal { get; set; }


        //Navigation Property
        public ICollection<Payment> PaymentsTo { get; set; }

        public ICollection<Payment> PaymentsFrom { get; set; }

        public ICollection<Beneficiary> Beneficiaries { get;}
    }
}
