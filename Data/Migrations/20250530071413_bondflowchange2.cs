using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetCareApis.Data.Migrations
{
    /// <inheritdoc />
    public partial class bondflowchange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondsDraws_ScheduleBonds",
                table: "BondsDraws");

            migrationBuilder.DropTable(
                name: "ScheduleBonds");

            migrationBuilder.DropTable(
                name: "BondsRecordsYear");

            migrationBuilder.RenameColumn(
                name: "schedule_id",
                table: "BondsDraws",
                newName: "bond_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_BondsDraws_schedule_id",
                table: "BondsDraws",
                newName: "IX_BondsDraws_bond_type_id");

            migrationBuilder.CreateTable(
                name: "BondTypes",
                columns: table => new
                {
                    type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bond_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_permium = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondTypes", x => x.type_id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BondsDraws_BondTypes",
                table: "BondsDraws",
                column: "bond_type_id",
                principalTable: "BondTypes",
                principalColumn: "type_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondsDraws_BondTypes",
                table: "BondsDraws");

            migrationBuilder.DropTable(
                name: "BondTypes");

            migrationBuilder.RenameColumn(
                name: "bond_type_id",
                table: "BondsDraws",
                newName: "schedule_id");

            migrationBuilder.RenameIndex(
                name: "IX_BondsDraws_bond_type_id",
                table: "BondsDraws",
                newName: "IX_BondsDraws_schedule_id");

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
                name: "ScheduleBonds",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    day = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_announced = table.Column<bool>(type: "bit", nullable: false),
                    is_premium = table.Column<bool>(type: "bit", nullable: false),
                    place = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleBonds", x => x.id);
                    table.ForeignKey(
                        name: "FK_ScheduleBonds_BondsRecordsYear",
                        column: x => x.year_id,
                        principalTable: "BondsRecordsYear",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleBonds_year_id",
                table: "ScheduleBonds",
                column: "year_id");

            migrationBuilder.AddForeignKey(
                name: "FK_BondsDraws_ScheduleBonds",
                table: "BondsDraws",
                column: "schedule_id",
                principalTable: "ScheduleBonds",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
