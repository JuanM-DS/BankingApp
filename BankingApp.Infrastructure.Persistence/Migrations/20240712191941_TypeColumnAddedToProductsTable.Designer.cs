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
    [Migration("20240712191941_TypeColumnAddedToProductsTable")]
    partial class TypeColumnAddedToProductsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankingApp.Core.Domain.Common.Product", b =>
                {
                    b.Property<int>("Id")
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

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.Beneficiary", b =>
                {
                    b.Property<string>("UserName")
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

                    b.HasKey("UserName", "AccountNumber");

                    b.HasIndex("AccountNumber");

                    b.ToTable("Beneficiaries", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.CreditCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 400100100L);

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 700100100L);

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

                    b.Property<double>("Principal")
                        .HasColumnType("float");

                    b.Property<int>("Term")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Loans", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<int?>("FromProductId")
                        .HasColumnType("int");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("datetime");

                    b.Property<int?>("ToProductId")
                        .HasColumnType("int");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FromProductId");

                    b.HasIndex("ToProductId");

                    b.ToTable("Payments", (string)null);
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.SavingsAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 100100100L);

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
                    b.HasOne("BankingApp.Core.Domain.Common.Product", "FromProduct")
                        .WithMany("PaymentsFrom")
                        .HasForeignKey("FromProductId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BankingApp.Core.Domain.Common.Product", "ToProduct")
                        .WithMany("PaymentsTo")
                        .HasForeignKey("ToProductId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("FromProduct");

                    b.Navigation("ToProduct");
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Common.Product", b =>
                {
                    b.Navigation("PaymentsFrom");

                    b.Navigation("PaymentsTo");
                });

            modelBuilder.Entity("BankingApp.Core.Domain.Entities.SavingsAccount", b =>
                {
                    b.Navigation("Beneficiaries");
                });
#pragma warning restore 612, 618
        }
    }
}
