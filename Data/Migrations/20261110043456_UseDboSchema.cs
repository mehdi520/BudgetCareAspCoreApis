using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetCareApis.Data.Migrations
{
    /// <inheritdoc />
    /// <remarks>
    /// Model now uses HasDefaultSchema("dbo"). All real tables already live in dbo, so this
    /// migration intentionally does nothing; it only updates the model snapshot.
    /// (The generated version tried to RenameTable every table into dbo.)
    /// Duplicate tables accidentally created in the "mehdi" schema are cleaned up by
    /// Doc/FixSchema_dbo.sql.
    /// </remarks>
    public partial class UseDboSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
