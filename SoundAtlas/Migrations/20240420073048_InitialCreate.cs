using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundAtlas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "instrument_categories",
                columns: table => new
                {
                    instrument_categories_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    classification1 = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    classification2 = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    classification3 = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    classification4 = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instrument_categories", x => x.instrument_categories_id);
                });

            migrationBuilder.CreateTable(
                name: "theories",
                columns: table => new
                {
                    theories_id = table.Column<int>(name: "theories_id ", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    melody_flg = table.Column<bool>(type: "INTEGER", nullable: false),
                    chrod_flg = table.Column<bool>(type: "INTEGER", nullable: false),
                    rhythm_flg = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theories", x => x.theories_id);
                });

            migrationBuilder.CreateTable(
                name: "virtual_instruments",
                columns: table => new
                {
                    virtual_instrument_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_virtual_instruments", x => x.virtual_instrument_id);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    words_id = table.Column<int>(name: "words_id ", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.words_id);
                });

            migrationBuilder.CreateTable(
                name: "instruments",
                columns: table => new
                {
                    instruments_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    instrument_categories_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instruments", x => x.instruments_id);
                    table.ForeignKey(
                        name: "FK_instruments_instrument_categories_instrument_categories_id",
                        column: x => x.instrument_categories_id,
                        principalTable: "instrument_categories",
                        principalColumn: "instrument_categories_id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "virtual_instrument_details",
                columns: table => new
                {
                    virtual_instrument_details_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    virtual_instruments_id = table.Column<int>(type: "INTEGER", nullable: false),
                    site_url = table.Column<string>(type: "TEXT", nullable: true),
                    version = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    last_updated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    image = table.Column<string>(type: "TEXT", nullable: true),
                    memo = table.Column<string>(type: "TEXT", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "theory_word_linkages",
                columns: table => new
                {
                    theory_word_linkages_id = table.Column<int>(name: "theory_word_linkages_id ", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    theories_id = table.Column<int>(type: "INTEGER", nullable: false),
                    words_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theory_word_linkages", x => x.theory_word_linkages_id);
                    table.ForeignKey(
                        name: "FK_theory_word_linkages_theories_theories_id",
                        column: x => x.theories_id,
                        principalTable: "theories",
                        principalColumn: "theories_id ",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_theory_word_linkages_words_words_id",
                        column: x => x.words_id,
                        principalTable: "words",
                        principalColumn: "words_id ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "word_details",
                columns: table => new
                {
                    word_details_id = table.Column<int>(name: "word_details_id ", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    words_id = table.Column<int>(type: "INTEGER", nullable: false),
                    memo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word_details", x => x.word_details_id);
                    table.ForeignKey(
                        name: "FK_word_details_words_words_id",
                        column: x => x.words_id,
                        principalTable: "words",
                        principalColumn: "words_id ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "virtual_instrument_presets",
                columns: table => new
                {
                    virtual_instrument_presets_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    virtual_instruments_id = table.Column<int>(type: "INTEGER", nullable: false),
                    instruments_id = table.Column<int>(type: "INTEGER", nullable: false),
                    preset_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    rate = table.Column<int>(type: "INTEGER", nullable: false),
                    melody_flg = table.Column<bool>(type: "INTEGER", nullable: false),
                    chord_flg = table.Column<bool>(type: "INTEGER", nullable: false),
                    bass_flg = table.Column<bool>(type: "INTEGER", nullable: false),
                    chrod_rhythm_flg = table.Column<bool>(type: "INTEGER", nullable: false),
                    percussion_flg = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_virtual_instrument_presets", x => x.virtual_instrument_presets_id);
                    table.ForeignKey(
                        name: "FK_virtual_instrument_presets_instruments_instruments_id",
                        column: x => x.instruments_id,
                        principalTable: "instruments",
                        principalColumn: "instruments_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_virtual_instrument_presets_virtual_instruments_virtual_instruments_id",
                        column: x => x.virtual_instruments_id,
                        principalTable: "virtual_instruments",
                        principalColumn: "virtual_instrument_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instrument_word_linkages",
                columns: table => new
                {
                    instrument_word_linkages_id = table.Column<int>(name: "instrument_word_linkages_id ", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    virtual_instrument_presets_id = table.Column<int>(type: "INTEGER", nullable: false),
                    words_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instrument_word_linkages", x => x.instrument_word_linkages_id);
                    table.ForeignKey(
                        name: "FK_instrument_word_linkages_virtual_instrument_presets_virtual_instrument_presets_id",
                        column: x => x.virtual_instrument_presets_id,
                        principalTable: "virtual_instrument_presets",
                        principalColumn: "virtual_instrument_presets_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_instrument_word_linkages_words_words_id",
                        column: x => x.words_id,
                        principalTable: "words",
                        principalColumn: "words_id ",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_instrument_word_linkages_virtual_instrument_presets_id",
                table: "instrument_word_linkages",
                column: "virtual_instrument_presets_id");

            migrationBuilder.CreateIndex(
                name: "IX_instrument_word_linkages_words_id",
                table: "instrument_word_linkages",
                column: "words_id");

            migrationBuilder.CreateIndex(
                name: "IX_instruments_instrument_categories_id",
                table: "instruments",
                column: "instrument_categories_id");

            migrationBuilder.CreateIndex(
                name: "IX_theory_details_theories_id",
                table: "theory_details",
                column: "theories_id");

            migrationBuilder.CreateIndex(
                name: "IX_theory_word_linkages_theories_id",
                table: "theory_word_linkages",
                column: "theories_id");

            migrationBuilder.CreateIndex(
                name: "IX_theory_word_linkages_words_id",
                table: "theory_word_linkages",
                column: "words_id");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_instrument_details_virtual_instruments_id",
                table: "virtual_instrument_details",
                column: "virtual_instruments_id");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_instrument_parameters_virtual_instrument_presets_id",
                table: "virtual_instrument_parameters",
                column: "virtual_instrument_presets_id");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_instrument_presets_instruments_id",
                table: "virtual_instrument_presets",
                column: "instruments_id");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_instrument_presets_virtual_instruments_id",
                table: "virtual_instrument_presets",
                column: "virtual_instruments_id");

            migrationBuilder.CreateIndex(
                name: "IX_word_details_words_id",
                table: "word_details",
                column: "words_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "instrument_word_linkages");

            migrationBuilder.DropTable(
                name: "theory_details");

            migrationBuilder.DropTable(
                name: "theory_word_linkages");

            migrationBuilder.DropTable(
                name: "virtual_instrument_details");

            migrationBuilder.DropTable(
                name: "virtual_instrument_parameters");

            migrationBuilder.DropTable(
                name: "word_details");

            migrationBuilder.DropTable(
                name: "theories");

            migrationBuilder.DropTable(
                name: "virtual_instrument_presets");

            migrationBuilder.DropTable(
                name: "words");

            migrationBuilder.DropTable(
                name: "instruments");

            migrationBuilder.DropTable(
                name: "virtual_instruments");

            migrationBuilder.DropTable(
                name: "instrument_categories");
        }
    }
}
