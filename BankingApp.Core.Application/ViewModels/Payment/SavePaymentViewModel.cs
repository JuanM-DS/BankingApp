using System.ComponentModel.DataAnnotations;

namespace BankingApp.Core.Application.ViewModels.Payment
{
    public class SavePaymentViewModel
    {
        public int? FromProductId { get; set; }

        public int? ToProductId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public double Amount { get; set; }

        [Required]
        public byte Type { get; set; }

        [Required]
        public byte ProductType { get; set; }

        [Required]
        public string UserName { get; set; }

    }
}
