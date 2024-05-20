using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundAtlas.Migrations
{
    /// <inheritdoc />
    public partial class add_abstract_and_worddetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Memo",
                table: "words",
                newName: "Abstract");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Abstract",
                table: "words",
                newName: "Memo");
        }
    }
}
