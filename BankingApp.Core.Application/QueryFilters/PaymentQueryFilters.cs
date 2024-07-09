using BankingApp.Core.Application.Enums;

namespace BankingApp.Core.Application.QueryFilters
{
    public class PaymentQueryFilters
    {
        public List<PaymentTypes?> PaymentTypes { get; set; }

        public DateTime? Time {  get; set; }

        public int? FromProductId { get; set; }

        public int? ToProductId { get; set; }
    }
}
