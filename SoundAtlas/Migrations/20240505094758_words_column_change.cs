using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundAtlas.Migrations
{
    /// <inheritdoc />
    public partial class words_column_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Detail",
                table: "words",
                newName: "detail");

            migrationBuilder.RenameColumn(
                name: "Abstract",
                table: "words",
                newName: "abstract");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "detail",
                table: "words",
                newName: "Detail");

            migrationBuilder.RenameColumn(
                name: "abstract",
                table: "words",
                newName: "Abstract");
        }
    }
}
