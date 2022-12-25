using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedAmbushes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ambush",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<long>(type: "bigint", nullable: false),
                    MapElementId = table.Column<long>(type: "bigint", nullable: false),
                    BossId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambush", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ambush_Agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ambush_Boss_BossId",
                        column: x => x.BossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ambush_MapElement_MapElementId",
                        column: x => x.MapElementId,
                        principalTable: "MapElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ambush_AgentId",
                table: "Ambush",
                column: "AgentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ambush_BossId",
                table: "Ambush",
                column: "BossId");

            migrationBuilder.CreateIndex(
                name: "IX_Ambush_MapElementId",
                table: "Ambush",
                column: "MapElementId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ambush");
        }
    }
}
