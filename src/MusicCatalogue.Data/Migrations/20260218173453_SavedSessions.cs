using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class SavedSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;");

            migrationBuilder.CreateTable(
                name: "SESSIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeOfDay = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSIONS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SESSION_ALBUMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    AlbumId = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION_ALBUMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SESSION_ALBUMS_ALBUMS_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "ALBUMS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SESSION_ALBUMS_SESSIONS_SessionId",
                        column: x => x.SessionId,
                        principalTable: "SESSIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SESSION_ALBUMS_AlbumId",
                table: "SESSION_ALBUMS",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_SESSION_ALBUMS_SessionId",
                table: "SESSION_ALBUMS",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SESSION_ALBUMS");

            migrationBuilder.DropTable(
                name: "SESSIONS");
        }
    }
}
