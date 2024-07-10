using BankingApp.Core.Domain.Common;


namespace BankingApp.Core.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int? FromProductId { get; set; }

        public int? ToProductId { get; set; }

        public double Amount { get; set; }

        public byte Type { get; set; }


        //Navigation Property
        public Product? FromProduct { get; set; }
        public Product? ToProduct { get; set; }
    }
}
