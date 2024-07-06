using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingproductTypeprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ProductType",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Payments");
        }
    }
}
