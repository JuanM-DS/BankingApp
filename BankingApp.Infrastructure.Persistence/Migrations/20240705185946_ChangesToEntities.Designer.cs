﻿// <auto-generated />
using System;
using BankingApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankingApp.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240705185946_ChangesToEntities")]
    partial class ChangesToEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.Beneficiary", b =>
                {
                    b.Property<string>("UserUserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BeneficiaryUserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserUserName", "BeneficiaryUserName", "AccountNumber");

                    b.HasIndex("AccountNumber");

                    b.ToTable("Beneficiaries", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.CreditCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<double>("CreditLimit")
                        .HasColumnType("float");

                    b.Property<byte>("CutoffDay")
                        .HasColumnType("tinyint");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("datetime");

                    b.Property<byte>("PaymentDay")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CreditCards", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<double>("Installment")
                        .HasColumnType("float");

                    b.Property<double>("InterestRate")
                        .HasColumnType("float");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("datetime");

                    b.Property<byte>("PaymentDay")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Loans", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<int?>("FromAccountId")
                        .HasColumnType("int");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("datetime");

                    b.Property<int?>("ToAccountId")
                        .HasColumnType("int");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FromAccountId");

                    b.HasIndex("ToAccountId");

                    b.ToTable("Payments", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.SavingsAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsPrincipal")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SavingsAccounts", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.Beneficiary", b =>
                {
                    b.HasOne("BankingApp.Core.Domain.Entities.SavingsAccount", "SavingsAccount")
                        .WithMany("Beneficiaries")
                        .HasForeignKey("AccountNumber")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("SavingsAccount");
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.Payment", b =>
                {
                    b.HasOne("BankingApp.Core.Domain.Entities.CreditCard", "FromCrediCard")
                        .WithMany("PaymentsFrom")
                        .HasForeignKey("FromAccountId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BankingApp.Core.Domain.Entities.Loan", "FromLoan")
                        .WithMany("PaymentsFrom")
                        .HasForeignKey("FromAccountId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BankingApp.Core.Domain.Entities.SavingsAccount", "FromAccount")
                        .WithMany("PaymentsFrom")
                        .HasForeignKey("FromAccountId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BankingApp.Core.Domain.Entities.CreditCard", "ToCreditCard")
                        .WithMany("PaymentsTo")
                        .HasForeignKey("ToAccountId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BankingApp.Core.Domain.Entities.Loan", "ToLoan")
                        .WithMany("PaymentsTo")
                        .HasForeignKey("ToAccountId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BankingApp.Core.Domain.Entities.SavingsAccount", "ToAccount")
                        .WithMany("PaymentsTo")
                        .HasForeignKey("ToAccountId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("FromAccount");

                    b.Navigation("FromCrediCard");

                    b.Navigation("FromLoan");

                    b.Navigation("ToAccount");

                    b.Navigation("ToCreditCard");

                    b.Navigation("ToLoan");
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.CreditCard", b =>
                {
                    b.Navigation("PaymentsFrom");

                    b.Navigation("PaymentsTo");
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.Loan", b =>
                {
                    b.Navigation("PaymentsFrom");

                    b.Navigation("PaymentsTo");
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.SavingsAccount", b =>
                {
                    b.Navigation("Beneficiaries");

                    b.Navigation("PaymentsFrom");

                    b.Navigation("PaymentsTo");
                });
#pragma warning restore 612, 618
        }
    }
}
