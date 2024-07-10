using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class Beneficiary : UserAuditableBaseEntity
    {

        public int AccountNumber { get; set; }

        //Navigation Properties
        public SavingsAccount SavingsAccount  { get; set; }

    }
}
