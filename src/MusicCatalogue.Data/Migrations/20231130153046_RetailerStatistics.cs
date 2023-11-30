using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    /// <inheritdoc />
    public partial class RetailerStatistics : Migration
    {
        [ExcludeFromCodeCoverage]
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RetailerStatistics",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Artists = table.Column<int>(type: "INTEGER", nullable: true),
                    Albums = table.Column<int>(type: "INTEGER", nullable: true),
                    Tracks = table.Column<int>(type: "INTEGER", nullable: true),
                    Spend = table.Column<decimal>(type: "TEXT", nullable: true),
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RetailerStatistics");
        }
    }
}
