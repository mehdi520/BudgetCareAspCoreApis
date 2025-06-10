using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetCareApis.Data.Migrations
{
    /// <inheritdoc />
    public partial class Userbond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "day",
                table: "BondsDraws",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "place",
                table: "BondsDraws",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "day",
                table: "BondsDraws");

            migrationBuilder.DropColumn(
                name: "place",
                table: "BondsDraws");
        }
    }
}
