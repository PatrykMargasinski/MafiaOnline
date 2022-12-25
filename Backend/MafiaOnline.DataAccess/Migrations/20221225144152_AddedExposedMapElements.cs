using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedExposedMapElements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "MapElement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ExposedMapElement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapElementId = table.Column<long>(type: "bigint", nullable: false),
                    ExposedByBossId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExposedMapElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExposedMapElement_Boss_ExposedByBossId",
                        column: x => x.ExposedByBossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExposedMapElement_MapElement_MapElementId",
                        column: x => x.MapElementId,
                        principalTable: "MapElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExposedMapElement_ExposedByBossId",
                table: "ExposedMapElement",
                column: "ExposedByBossId");

            migrationBuilder.CreateIndex(
                name: "IX_ExposedMapElement_MapElementId",
                table: "ExposedMapElement",
                column: "MapElementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExposedMapElement");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "MapElement");
        }
    }
}
