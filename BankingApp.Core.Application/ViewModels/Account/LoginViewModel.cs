using System.ComponentModel.DataAnnotations;

namespace BankingApp.Core.Application.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe indicar su nombre de usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Debe indicar su contraseña")]
        public string Password { get; set; }
    }
}
