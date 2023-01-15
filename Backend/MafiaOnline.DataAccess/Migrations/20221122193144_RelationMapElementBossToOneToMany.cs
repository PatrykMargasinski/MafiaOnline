using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class RelationMapElementBossToOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MapElement_BossId",
                table: "MapElement");

            migrationBuilder.CreateIndex(
                name: "IX_MapElement_BossId",
                table: "MapElement",
                column: "BossId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MapElement_BossId",
                table: "MapElement");

            migrationBuilder.CreateIndex(
                name: "IX_MapElement_BossId",
                table: "MapElement",
                column: "BossId",
                unique: true,
                filter: "[BossId] IS NOT NULL");
        }
    }
}
