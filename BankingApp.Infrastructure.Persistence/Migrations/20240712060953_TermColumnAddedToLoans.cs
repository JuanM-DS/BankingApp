using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TermColumnAddedToLoans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Term",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Term",
                table: "Loans");
        }
    }
}
