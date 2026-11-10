using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetCareApis.Data.Migrations
{
    /// <inheritdoc />
    /// <remarks>
    /// Only creates the NoteAttachment table. The model snapshot was also brought up to date with
    /// UserBonds / UserWonBonds, which already exist in the database (they were created outside migrations).
    /// </remarks>
    public partial class NoteAttachmentsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoteAttachment",
                columns: table => new
                {
                    attachment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    note_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    file_path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    content_type = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteAttachment", x => x.attachment_id);
                    table.ForeignKey(
                        name: "FK_NoteAttachment_Note",
                        column: x => x.note_id,
                        principalTable: "Notes",
                        principalColumn: "note_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteAttachment_note_id",
                table: "NoteAttachment",
                column: "note_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteAttachment");
        }
    }
}
