using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class Spending : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Purchased",
                table: "TRACKS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ALBUMS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Purchased",
                table: "ALBUMS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetailerId",
                table: "ALBUMS",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Retailers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retailers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALBUMS_RetailerId",
                table: "ALBUMS",
                column: "RetailerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ALBUMS_Retailers_RetailerId",
                table: "ALBUMS",
                column: "RetailerId",
                principalTable: "Retailers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ALBUMS_Retailers_RetailerId",
                table: "ALBUMS");

            migrationBuilder.DropTable(
                name: "Retailers");

            migrationBuilder.DropIndex(
                name: "IX_ALBUMS_RetailerId",
                table: "ALBUMS");

            migrationBuilder.DropColumn(
                name: "Purchased",
                table: "TRACKS");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ALBUMS");

            migrationBuilder.DropColumn(
                name: "Purchased",
                table: "ALBUMS");

            migrationBuilder.DropColumn(
                name: "RetailerId",
                table: "ALBUMS");
        }
    }
}
