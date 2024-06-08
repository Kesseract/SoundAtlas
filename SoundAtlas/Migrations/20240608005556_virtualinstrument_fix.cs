using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundAtlas.Migrations
{
    /// <inheritdoc />
    public partial class virtualinstrument_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "virtual_instrument_details");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "virtual_instruments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_updated",
                table: "virtual_instruments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "memo",
                table: "virtual_instruments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "site_url",
                table: "virtual_instruments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "version",
                table: "virtual_instruments",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "virtual_instruments");

            migrationBuilder.DropColumn(
                name: "last_updated",
                table: "virtual_instruments");

            migrationBuilder.DropColumn(
                name: "memo",
                table: "virtual_instruments");

            migrationBuilder.DropColumn(
                name: "site_url",
                table: "virtual_instruments");

            migrationBuilder.DropColumn(
                name: "version",
                table: "virtual_instruments");

            migrationBuilder.CreateTable(
                name: "virtual_instrument_details",
                columns: table => new
                {
                    virtual_instrument_details_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    virtual_instruments_id = table.Column<int>(type: "INTEGER", nullable: false),
                    image = table.Column<string>(type: "TEXT", nullable: true),
                    last_updated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    memo = table.Column<string>(type: "TEXT", nullable: true),
                    site_url = table.Column<string>(type: "TEXT", nullable: true),
                    version = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_virtual_instrument_details", x => x.virtual_instrument_details_id);
                    table.ForeignKey(
                        name: "FK_virtual_instrument_details_virtual_instruments_virtual_instruments_id",
                        column: x => x.virtual_instruments_id,
                        principalTable: "virtual_instruments",
                        principalColumn: "virtual_instrument_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_virtual_instrument_details_virtual_instruments_id",
                table: "virtual_instrument_details",
                column: "virtual_instruments_id");
        }
    }
}
