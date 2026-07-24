using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChecklistApp.Migrations
{
    /// <inheritdoc />
    public partial class ChecklistColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChecklistColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Red = table.Column<float>(type: "REAL", nullable: false),
                    Green = table.Column<float>(type: "REAL", nullable: false),
                    Blue = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistColors", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistColors");
        }
    }
}
