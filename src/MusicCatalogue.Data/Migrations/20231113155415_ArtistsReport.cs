using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class ArtistsReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistStatistics",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "ArtistStatistics");
        }
    }
}
