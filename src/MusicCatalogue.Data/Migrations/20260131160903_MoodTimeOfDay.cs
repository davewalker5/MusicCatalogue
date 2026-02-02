using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class MoodTimeOfDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AfternoonWeight",
                table: "MOODS",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EveningWeight",
                table: "MOODS",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LateWeight",
                table: "MOODS",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MorningWeight",
                table: "MOODS",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfternoonWeight",
                table: "MOODS");

            migrationBuilder.DropColumn(
                name: "EveningWeight",
                table: "MOODS");

            migrationBuilder.DropColumn(
                name: "LateWeight",
                table: "MOODS");

            migrationBuilder.DropColumn(
                name: "MorningWeight",
                table: "MOODS");
        }
    }
}
