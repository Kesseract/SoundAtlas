using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundAtlas.Migrations
{
    /// <inheritdoc />
    public partial class add_word_details : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "word_details",
                columns: table => new
                {
                    word_details_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    words_id = table.Column<int>(type: "INTEGER", nullable: false),
                    detail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word_details", x => x.word_details_id);
                    table.ForeignKey(
                        name: "FK_word_details_words_words_id",
                        column: x => x.words_id,
                        principalTable: "words",
                        principalColumn: "words_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_word_details_words_id",
                table: "word_details",
                column: "words_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "word_details");
        }
    }
}
