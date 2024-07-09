using BankingApp.Core.Application.Enums;

namespace BankingApp.Core.Application.DTOs.User
{
    public class ApplicationUserDTO
    {
        public string? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string IdCard { get; set; }

        public byte Status { get; set; }

        public string Password { get; set; } = null!;

        public List<RoleTypes> Roles { get; set; }

        public string? PhotoUrl { get; set; }

    }
}
