namespace BankingApp.Core.Application.DTOs.Account.ResetPassword
{
    public class ResetPasswordRequestDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}
