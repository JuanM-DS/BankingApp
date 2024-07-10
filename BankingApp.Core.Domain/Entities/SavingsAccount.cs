using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class SavingsAccount: Product
    {
        public bool IsPrincipal { get; set; }
        public ICollection<Beneficiary> Beneficiaries { get;}
    }
}
