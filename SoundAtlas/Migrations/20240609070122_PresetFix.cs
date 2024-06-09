using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundAtlas.Migrations
{
    /// <inheritdoc />
    public partial class PresetFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "virtual_instrument_parameters");

            migrationBuilder.RenameColumn(
                name: "preset_name",
                table: "virtual_instrument_presets",
                newName: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "virtual_instrument_presets",
                newName: "preset_name");

            migrationBuilder.CreateTable(
                name: "virtual_instrument_parameters",
                columns: table => new
                {
                    virtual_instrument_parameters_id = table.Column<int>(name: "virtual_instrument_parameters_id ", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    virtual_instrument_presets_id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    value = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_virtual_instrument_parameters", x => x.virtual_instrument_parameters_id);
                    table.ForeignKey(
                        name: "FK_virtual_instrument_parameters_virtual_instrument_presets_virtual_instrument_presets_id",
                        column: x => x.virtual_instrument_presets_id,
                        principalTable: "virtual_instrument_presets",
                        principalColumn: "virtual_instrument_presets_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_virtual_instrument_parameters_virtual_instrument_presets_id",
                table: "virtual_instrument_parameters",
                column: "virtual_instrument_presets_id");
        }
    }
}
