using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundAtlas.Migrations
{
    /// <inheritdoc />
    public partial class Required : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "words_id ",
                table: "words",
                newName: "words_id");

            migrationBuilder.RenameColumn(
                name: "word_details_id ",
                table: "word_details",
                newName: "word_details_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "words_id",
                table: "words",
                newName: "words_id ");

            migrationBuilder.RenameColumn(
                name: "word_details_id",
                table: "word_details",
                newName: "word_details_id ");
        }
    }
}
