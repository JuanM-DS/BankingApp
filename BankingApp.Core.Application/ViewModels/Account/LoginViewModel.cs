using System.ComponentModel.DataAnnotations;

namespace BankingApp.Core.Application.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
