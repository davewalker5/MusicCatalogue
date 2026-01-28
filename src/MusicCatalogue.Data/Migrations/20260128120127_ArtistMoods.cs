using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class ArtistMoods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ARTIST_MOODS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArtistId = table.Column<int>(type: "INTEGER", nullable: false),
                    MoodId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ARTIST_MOODS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ARTIST_MOODS_ARTISTS_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "ARTISTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ARTIST_MOODS_MOODS_MoodId",
                        column: x => x.MoodId,
                        principalTable: "MOODS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ARTIST_MOODS_ArtistId",
                table: "ARTIST_MOODS",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ARTIST_MOODS_MoodId",
                table: "ARTIST_MOODS",
                column: "MoodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ARTIST_MOODS");
        }
    }
}
