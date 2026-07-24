using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChecklistApp.Migrations
{
    /// <inheritdoc />
    public partial class ChecklistCustomColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomColorId",
                table: "Checklists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomColorId",
                table: "Checklists");
        }
    }
}
