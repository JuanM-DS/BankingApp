using BankingApp.Core.Application.Enums;
using BankingApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Infrastructure.Identity.Contexts
{
    public class BankingAppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public BankingAppIdentityDbContext(DbContextOptions option) : base(option)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(e =>
            {
                e.ToTable(name : "Users");
                e.Property(x => x.Status);
                e.Property(x => x.FirstName);
                e.Property(x => x.LastName);
                e.Property(x => x.IdCard);
            });

            builder.Entity<IdentityRole>(e =>
            {
                e.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable(name: "UserRole");
            });

            builder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable(name: "UserLogin");
            });

            base.OnModelCreating(builder);
        }
    }
}
