using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditCards_FromAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditCards_ToAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Loans_FromAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Loans_ToAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SavingsAccounts_FromAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SavingsAccounts_ToAccountId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "ToAccountId",
                table: "Payments",
                newName: "ToProductId");

            migrationBuilder.RenameColumn(
                name: "FromAccountId",
                table: "Payments",
                newName: "FromProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ToAccountId",
                table: "Payments",
                newName: "IX_Payments_ToProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_FromAccountId",
                table: "Payments",
                newName: "IX_Payments_FromProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CreditCards_FromProductId",
                table: "Payments",
                column: "FromProductId",
                principalTable: "CreditCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CreditCards_ToProductId",
                table: "Payments",
                column: "ToProductId",
                principalTable: "CreditCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Loans_FromProductId",
                table: "Payments",
                column: "FromProductId",
                principalTable: "Loans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Loans_ToProductId",
                table: "Payments",
                column: "ToProductId",
                principalTable: "Loans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SavingsAccounts_FromProductId",
                table: "Payments",
                column: "FromProductId",
                principalTable: "SavingsAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SavingsAccounts_ToProductId",
                table: "Payments",
                column: "ToProductId",
                principalTable: "SavingsAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditCards_FromProductId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditCards_ToProductId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Loans_FromProductId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Loans_ToProductId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SavingsAccounts_FromProductId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SavingsAccounts_ToProductId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "ToProductId",
                table: "Payments",
                newName: "ToAccountId");

            migrationBuilder.RenameColumn(
                name: "FromProductId",
                table: "Payments",
                newName: "FromAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ToProductId",
                table: "Payments",
                newName: "IX_Payments_ToAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_FromProductId",
                table: "Payments",
                newName: "IX_Payments_FromAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CreditCards_FromAccountId",
                table: "Payments",
                column: "FromAccountId",
                principalTable: "CreditCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CreditCards_ToAccountId",
                table: "Payments",
                column: "ToAccountId",
                principalTable: "CreditCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Loans_FromAccountId",
                table: "Payments",
                column: "FromAccountId",
                principalTable: "Loans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Loans_ToAccountId",
                table: "Payments",
                column: "ToAccountId",
                principalTable: "Loans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SavingsAccounts_FromAccountId",
                table: "Payments",
                column: "FromAccountId",
                principalTable: "SavingsAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SavingsAccounts_ToAccountId",
                table: "Payments",
                column: "ToAccountId",
                principalTable: "SavingsAccounts",
                principalColumn: "Id");
        }
    }
}
