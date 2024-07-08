using System.ComponentModel.DataAnnotations;

namespace BankingApp.Core.Application.ViewModels.Account
{
    public class ConfirmAccountViewModel
    {
        [Required]
        public string UserId { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}
