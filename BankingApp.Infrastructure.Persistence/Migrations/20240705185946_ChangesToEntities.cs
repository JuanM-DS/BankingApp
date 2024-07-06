using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangesToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditCards_ToCreditCardId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Loans_ToLoanId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ToCreditCardId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ToLoanId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ToCreditCardId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ToLoanId",
                table: "Payments");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ToCreditCardId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToLoanId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ToCreditCardId",
                table: "Payments",
                column: "ToCreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ToLoanId",
                table: "Payments",
                column: "ToLoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CreditCards_ToCreditCardId",
                table: "Payments",
                column: "ToCreditCardId",
                principalTable: "CreditCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Loans_ToLoanId",
                table: "Payments",
                column: "ToLoanId",
                principalTable: "Loans",
                principalColumn: "Id");
        }
    }
}
