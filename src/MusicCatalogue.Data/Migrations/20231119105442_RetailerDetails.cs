using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class RetailerDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                table: "RETAILERS",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "RETAILERS");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "RETAILERS");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "RETAILERS");

            migrationBuilder.DropColumn(
                name: "County",
                table: "RETAILERS");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "RETAILERS");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "RETAILERS");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "RETAILERS");

            migrationBuilder.DropColumn(
                name: "Town",
                table: "RETAILERS");

            migrationBuilder.DropColumn(
                name: "WebSite",
                table: "RETAILERS");
        }
    }
}
