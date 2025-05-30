using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetCareApis.Data.Migrations
{
    /// <inheritdoc />
    public partial class bondflowchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ScheduleBonds_year_id",
                table: "ScheduleBonds",
                column: "year_id");

            migrationBuilder.CreateIndex(
                name: "IX_DrawWinsBonds_draw_id",
                table: "DrawWinsBonds",
                column: "draw_id");

            migrationBuilder.CreateIndex(
                name: "IX_BondsDraws_schedule_id",
                table: "BondsDraws",
                column: "schedule_id");

            migrationBuilder.AddForeignKey(
                name: "FK_BondsDraws_ScheduleBonds",
                table: "BondsDraws",
                column: "schedule_id",
                principalTable: "ScheduleBonds",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrawWinsBonds_BondsDraws",
                table: "DrawWinsBonds",
                column: "draw_id",
                principalTable: "BondsDraws",
                principalColumn: "draw_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleBonds_BondsRecordsYear",
                table: "ScheduleBonds",
                column: "year_id",
                principalTable: "BondsRecordsYear",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondsDraws_ScheduleBonds",
                table: "BondsDraws");

            migrationBuilder.DropForeignKey(
                name: "FK_DrawWinsBonds_BondsDraws",
                table: "DrawWinsBonds");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleBonds_BondsRecordsYear",
                table: "ScheduleBonds");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleBonds_year_id",
                table: "ScheduleBonds");

            migrationBuilder.DropIndex(
                name: "IX_DrawWinsBonds_draw_id",
                table: "DrawWinsBonds");

            migrationBuilder.DropIndex(
                name: "IX_BondsDraws_schedule_id",
                table: "BondsDraws");
        }
    }
}
