using BankingApp.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte Status { get; set; }

        public string IdCard { get; set; }
    }
}
