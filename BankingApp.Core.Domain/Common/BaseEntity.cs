namespace BankingApp.Core.Domain.Common
{
    public abstract class BaseEntity : AuditableBaseEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

}
