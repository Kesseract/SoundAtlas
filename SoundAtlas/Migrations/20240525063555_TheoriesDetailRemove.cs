using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundAtlas.Migrations
{
    /// <inheritdoc />
    public partial class TheoriesDetailRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "theory_details");

            migrationBuilder.AddColumn<string>(
                name: "abstract",
                table: "theories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "detail",
                table: "theories",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "abstract",
                table: "theories");

            migrationBuilder.DropColumn(
                name: "detail",
                table: "theories");

            migrationBuilder.CreateTable(
                name: "theory_details",
                columns: table => new
                {
                    theory_details_id = table.Column<int>(name: "theory_details_id ", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    theories_id = table.Column<int>(type: "INTEGER", nullable: false),
                    memo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theory_details", x => x.theory_details_id);
                    table.ForeignKey(
                        name: "FK_theory_details_theories_theories_id",
                        column: x => x.theories_id,
                        principalTable: "theories",
                        principalColumn: "theories_id ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_theory_details_theories_id",
                table: "theory_details",
                column: "theories_id");
        }
    }
}
