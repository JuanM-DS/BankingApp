using BankingApp.Core.Domain.Common;
using BankingApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext (DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SavingsAccount> SavingsAccounts { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<UserAuditableBaseEntity>())
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Tables
            modelBuilder.Entity<Beneficiary>()
                .ToTable("Beneficiaries");

            modelBuilder.Entity<CreditCard>()
                .ToTable("CreditCards");

            modelBuilder.Entity<Loan>()
                .ToTable("Loans");

            modelBuilder.Entity<Payment>()
                .ToTable("Payments");

            modelBuilder.Entity<SavingsAccount>()
                .ToTable("SavingsAccounts");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<CreditCard>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Loan>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<SavingsAccount>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Beneficiary>()
                .HasKey(b => new {b.UserName,b.BeneficiaryUserName,b.AccountNumber});

            #endregion

            #region Relationships
            modelBuilder.Entity<SavingsAccount>()
                .HasMany<Payment>(s => s.PaymentsFrom)
                .WithOne(p => p.FromAccount)
                .HasForeignKey(p => p.FromProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SavingsAccount>()
                .HasMany<Payment>(s => s.PaymentsTo)
                .WithOne(p => p.ToAccount)
                .HasForeignKey(p => p.ToProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SavingsAccount>()
                .HasMany<Beneficiary>(s => s.Beneficiaries)
                .WithOne(b => b.SavingsAccount)
                .HasForeignKey(b => b.AccountNumber)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CreditCard>()
               .HasMany<Payment>(s => s.PaymentsTo)
               .WithOne(b => b.ToCreditCard)
               .HasForeignKey(b => b.ToProductId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CreditCard>()
               .HasMany<Payment>(s => s.PaymentsFrom)
               .WithOne(b => b.FromCrediCard)
               .HasForeignKey(b => b.FromProductId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Loan>()
               .HasMany<Payment>(s => s.PaymentsFrom)
               .WithOne(b => b.FromLoan)
               .HasForeignKey(b => b.FromProductId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Loan>()
               .HasMany<Payment>(s => s.PaymentsTo)
               .WithOne(b => b.ToLoan)
               .HasForeignKey(b => b.ToProductId)
               .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Property configurations

            #region CreditCard

            modelBuilder.Entity<CreditCard>(creditCard =>
            {
                creditCard.Property(c => c.Balance)
                .IsRequired();

                creditCard.Property(c => c.CreditLimit)
                .IsRequired();

                creditCard.Property(c => c.CutoffDay)
                .IsRequired();

                creditCard.Property(c => c.PaymentDay)
                .IsRequired();

                creditCard.Property(l => l.CreatedBy)
                .IsRequired();

                creditCard.Property(l => l.CreatedTime)
                .HasColumnType("datetime");

                creditCard.Property(l => l.LastModifiedBy);

                creditCard.Property(l => l.LastModifiedTime)
                .HasColumnType("datetime");
            });
            #endregion

            #region Loan
            modelBuilder.Entity<Loan>(loan =>
            {
                loan.Property(l => l.Balance)
                .IsRequired();

                loan.Property(l => l.InterestRate)
                .IsRequired();

                loan.Property(l => l.Installment)
                .IsRequired();

                loan.Property(l => l.PaymentDay)
                .IsRequired();

                loan.Property(l => l.CreatedBy)
                .IsRequired();

                loan.Property(l => l.CreatedTime)
                .HasColumnType("datetime");

                loan.Property(l => l.LastModifiedBy);

                loan.Property(l => l.LastModifiedTime)
                .HasColumnType("datetime");
            });

            #endregion

            #region Payment
            modelBuilder.Entity<Payment>(payment =>
            {
                payment.Property(p => p.FromProductId);

                payment.Property(p => p.ToProductId);

                payment.Property(p => p.Amount)
                .IsRequired();

                payment.Property(p => p.Type)
                .IsRequired();

                payment.Property(l => l.CreatedBy)
                .IsRequired();

                payment.Property(l => l.CreatedTime)
                .HasColumnType("datetime");

                payment.Property(l => l.LastModifiedBy);

                payment.Property(l => l.LastModifiedTime)
                .HasColumnType("datetime");
            });

            #endregion

            #region Savings Account
            modelBuilder.Entity<SavingsAccount>(savingsAcount =>
            {
                savingsAcount.Property(s => s.Balance)
                .IsRequired();

                savingsAcount.Property(s => s.IsPrincipal)
                .IsRequired();

                savingsAcount.Property(l => l.CreatedBy)
                .IsRequired();

                savingsAcount.Property(l => l.CreatedTime)
                .HasColumnType("datetime");

                savingsAcount.Property(l => l.LastModifiedBy);

                savingsAcount.Property(l => l.LastModifiedTime)
                .HasColumnType("datetime");
            });

            #endregion
            #endregion
        }
    }
}
