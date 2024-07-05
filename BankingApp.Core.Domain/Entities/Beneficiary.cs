using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class Beneficiary : UserAuditableBaseEntity
    {
        public string UserUserName { get; set; }

        public string BeneficiaryUserName {  get; set; }

        public int AccountNumber { get; set; }

        //Navigation Properties
        public SavingsAccount SavingsAccount  { get; set; }

    }
}
