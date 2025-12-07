using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class GenresEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "ALBUMS",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GENRES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GENRES", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALBUMS_GenreId",
                table: "ALBUMS",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_ALBUMS_GENRES_GenreId",
                table: "ALBUMS",
                column: "GenreId",
                principalTable: "GENRES",
                principalColumn: "Id");

            var sql = MigrationUtilities.ReadMigrationSqlScript("GenreDataMigration.sql");
            migrationBuilder.Sql(sql);

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "ALBUMS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ALBUMS_GENRES_GenreId",
                table: "ALBUMS");

            migrationBuilder.DropTable(
                name: "GENRES");

            migrationBuilder.DropIndex(
                name: "IX_ALBUMS_GenreId",
                table: "ALBUMS");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "ALBUMS");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "ALBUMS",
                type: "TEXT",
                nullable: true);
        }
    }
}
