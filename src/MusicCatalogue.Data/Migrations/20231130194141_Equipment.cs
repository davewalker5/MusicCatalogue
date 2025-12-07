using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class Equipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EQUIPMENT_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENT_TYPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MANUFACTURERS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MANUFACTURERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPMENT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ManufacturerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: true),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    IsWishListItem = table.Column<bool>(type: "INTEGER", nullable: true),
                    Purchased = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    RetailerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EQUIPMENT_EQUIPMENT_TYPES_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EQUIPMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EQUIPMENT_MANUFACTURERS_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "MANUFACTURERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EQUIPMENT_RETAILERS_RetailerId",
                        column: x => x.RetailerId,
                        principalTable: "RETAILERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENT_EquipmentTypeId",
                table: "EQUIPMENT",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENT_ManufacturerId",
                table: "EQUIPMENT",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENT_RetailerId",
                table: "EQUIPMENT",
                column: "RetailerId");

            var sql = MigrationUtilities.ReadMigrationSqlScript("EquipmentMigration.sql");
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EQUIPMENT");

            migrationBuilder.DropTable(
                name: "EQUIPMENT_TYPES");

            migrationBuilder.DropTable(
                name: "MANUFACTURERS");
        }
    }
}
