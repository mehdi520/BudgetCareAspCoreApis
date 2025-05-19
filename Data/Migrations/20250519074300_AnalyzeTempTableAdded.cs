using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetCareApis.Data.Migrations
{
    /// <inheritdoc />
    public partial class AnalyzeTempTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BondsDraws",
                columns: table => new
                {
                    draw_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    schedule_id = table.Column<int>(type: "int", nullable: false),
                    draw_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    draw_no = table.Column<int>(type: "int", nullable: false),
                    first_prize_worth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    second_prize_worth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    third_prize_worth = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondsDraws", x => x.draw_id);
                });

            migrationBuilder.CreateTable(
                name: "BondsRecordsYear",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondsRecordsYear", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DrawWinsBonds",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    draw_id = table.Column<int>(type: "int", nullable: false),
                    bound_no = table.Column<int>(type: "int", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawWinsBonds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleBonds",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_premium = table.Column<bool>(type: "bit", nullable: false),
                    day = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    place = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_announced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleBonds", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BondsDraws");

            migrationBuilder.DropTable(
                name: "BondsRecordsYear");

            migrationBuilder.DropTable(
                name: "DrawWinsBonds");

            migrationBuilder.DropTable(
                name: "ScheduleBonds");
        }
    }
}
