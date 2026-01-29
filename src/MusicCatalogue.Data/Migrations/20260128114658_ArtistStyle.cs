using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class ArtistStyle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Energy",
                table: "ARTISTS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ensemble",
                table: "ARTISTS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Intimacy",
                table: "ARTISTS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Vocals",
                table: "ARTISTS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Warmth",
                table: "ARTISTS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MOODS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOODS", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOODS");

            migrationBuilder.DropColumn(
                name: "Energy",
                table: "ARTISTS");

            migrationBuilder.DropColumn(
                name: "Ensemble",
                table: "ARTISTS");

            migrationBuilder.DropColumn(
                name: "Intimacy",
                table: "ARTISTS");

            migrationBuilder.DropColumn(
                name: "Vocals",
                table: "ARTISTS");

            migrationBuilder.DropColumn(
                name: "Warmth",
                table: "ARTISTS");
        }
    }
}
