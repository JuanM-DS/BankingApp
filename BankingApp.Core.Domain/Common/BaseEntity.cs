namespace BankingApp.Core.Domain.Common
{
    public abstract class BaseEntity : UserAuditableBaseEntity
    {
        public virtual int Id { get; set; }
    }

}
