using BankingApp.Core.Application.Enums;

namespace BankingApp.Core.Application.QueryFilters
{
    public class UserQueryFilter
    {
        public string? Email { get; set; }

        public string? IdCard { get; set; }

        public byte? Status { get; set; }

        public RoleTypes? Role { get; set; }
    }
}
