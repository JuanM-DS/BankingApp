using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class Payment : UserAuditableBaseEntity
    {
        public Guid Id { get; set; }
        public int? FromProductId { get; set; }
        public int? ToProductId { get; set; }
        public double Amount { get; set; }
        public byte Type { get; set; }

    }
}
