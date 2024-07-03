namespace BankingApp.Core.Domain.Common
{
    public abstract class AuditableBaseEntity
    {
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual string? LastModifiedBy { get; set; }
        public virtual DateTime? LastModifiedTime { get; set; }
    }

}
