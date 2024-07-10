using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class SavingsAccount: BaseEntity
    {
        public double Balance { get; set; }
        public bool IsPrincipal { get; set; }
        public ICollection<Beneficiary> Beneficiaries { get;}
    }
}
