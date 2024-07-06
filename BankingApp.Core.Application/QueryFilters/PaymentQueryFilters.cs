using BankingApp.Core.Application.Enums;

namespace BankingApp.Core.Application.QueryFilters
{
    public class PaymentQueryFilters
    {
        public PaymentTypes? PaymentTypes { get; set; }

        public ProductTypes? ProductTypes { get; set; }

        public DateTime? Time {  get; set; }

        public int? FromProductId { get; set; }

        public int? ToProductId { get; set; }
    }
}
