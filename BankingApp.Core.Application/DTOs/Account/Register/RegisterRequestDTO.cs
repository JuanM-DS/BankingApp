using BankingApp.Core.Application.Enums;

namespace BankingApp.Core.Application.DTOs.Account.Register
{
    public class RegisterRequestDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string IdCard { get; set; }

        public RoleTypes role {  get; set; }

        public string Password { get; set; }

        public string Origin { get; set; }
    }
}
