using BankingApp.Core.Application.Enums;
using BankingApp.Core.Domain.Common;
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<ApplicationUser>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedTime = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedTime = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(e =>
            {
                e.ToTable(name : "Users");
                e.Property(x => x.Status)
                .HasDefaultValue((int)UserStatus.Active);
                e.Property(x => x.PhotoUrl);
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
