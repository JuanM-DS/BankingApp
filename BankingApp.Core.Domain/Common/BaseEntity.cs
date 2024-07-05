namespace BankingApp.Core.Domain.Common
{
    public abstract class BaseEntity : UserAuditableBaseEntity
    {
        public int Id { get; set; }
    }

}
