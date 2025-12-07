using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class GenresReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenreStatistics",
                columns: table => new
                {
                    Genre = table.Column<string>(type: "TEXT", nullable: false),
                    Artists = table.Column<int>(type: "INTEGER", nullable: true),
                    Albums = table.Column<int>(type: "INTEGER", nullable: true),
                    Tracks = table.Column<int>(type: "INTEGER", nullable: true),
                    Spend = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreStatistics");
        }
    }
}
