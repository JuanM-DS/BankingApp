using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addingNavigatorProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Balance",
                table: "SavingsAccounts",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ToAccountId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ToAccountId",
                table: "Payments",
                column: "ToAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SavingsAccounts_ToAccountId",
                table: "Payments",
                column: "ToAccountId",
                principalTable: "SavingsAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SavingsAccounts_ToAccountId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ToAccountId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                table: "SavingsAccounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "ToAccountId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
