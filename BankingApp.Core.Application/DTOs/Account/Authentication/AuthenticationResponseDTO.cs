using BankingApp.Core.Application.DTOs.User;

namespace BankingApp.Core.Application.DTOs.Account.Authentication
{
    public class AuthenticationResponseDTO
    {
        public ApplicationUserDTO UserDTO { get; set; }

        public bool Success { get; set; }

        public string? Error { get; set; }
    }
}
