using Microsoft.AspNetCore.Identity;

namespace BankingApp.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte Status { get; set; }

        public string IdCard { get; set; }

        public string? PhotoUrl { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedTime { get; set; }
    }
}
