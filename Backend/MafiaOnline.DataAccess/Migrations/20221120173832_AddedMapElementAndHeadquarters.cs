using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedMapElementAndHeadquarters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapElement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<long>(type: "bigint", nullable: false),
                    Y = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    BossId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapElement_Boss_BossId",
                        column: x => x.BossId,
                        principalTable: "Boss",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Headquarters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapElementId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BossId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headquarters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headquarters_Boss_BossId",
                        column: x => x.BossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Headquarters_MapElement_MapElementId",
                        column: x => x.MapElementId,
                        principalTable: "MapElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MapElement",
                columns: new[] { "Id", "BossId", "Type", "X", "Y" },
                values: new object[] { 1L, 1L, 1, 2L, 1L });

            migrationBuilder.InsertData(
                table: "MapElement",
                columns: new[] { "Id", "BossId", "Type", "X", "Y" },
                values: new object[] { 2L, 2L, 1, 14L, 1L });

            migrationBuilder.InsertData(
                table: "Headquarters",
                columns: new[] { "Id", "BossId", "MapElementId", "Name" },
                values: new object[] { 1L, 1L, 1L, "The house of Patricio Rico" });

            migrationBuilder.InsertData(
                table: "Headquarters",
                columns: new[] { "Id", "BossId", "MapElementId", "Name" },
                values: new object[] { 2L, 2L, 2L, "Margherita rules here" });

            migrationBuilder.CreateIndex(
                name: "IX_Headquarters_BossId",
                table: "Headquarters",
                column: "BossId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Headquarters_MapElementId",
                table: "Headquarters",
                column: "MapElementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapElement_BossId",
                table: "MapElement",
                column: "BossId",
                unique: true,
                filter: "[BossId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headquarters");

            migrationBuilder.DropTable(
                name: "MapElement");
        }
    }
}
