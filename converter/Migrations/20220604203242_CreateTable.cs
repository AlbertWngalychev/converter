using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace converter.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "convert",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    file_name = table.Column<string>(type: "TEXT", nullable: false),
                    content_type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_convert", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "result",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "VARCHAR (255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_result", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    date_time = table.Column<long>(type: "INTEGER", nullable: false),
                    result_id = table.Column<long>(type: "INTEGER", nullable: true),
                    convert_id = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_status_convert_convert_id",
                        column: x => x.convert_id,
                        principalTable: "convert",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_status_result_result_id",
                        column: x => x.result_id,
                        principalTable: "result",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_convert_id",
                table: "convert",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_result_id",
                table: "result",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_status_convert_id",
                table: "status",
                column: "convert_id");

            migrationBuilder.CreateIndex(
                name: "IX_status_id",
                table: "status",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_status_result_id",
                table: "status",
                column: "result_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.DropTable(
                name: "convert");

            migrationBuilder.DropTable(
                name: "result");
        }
    }
}
