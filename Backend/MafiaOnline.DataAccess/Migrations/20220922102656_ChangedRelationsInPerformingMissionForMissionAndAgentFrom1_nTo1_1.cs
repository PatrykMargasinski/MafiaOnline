using Microsoft.EntityFrameworkCore.Migrations;

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class ChangedRelationsInPerformingMissionForMissionAndAgentFrom1_nTo1_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PerformingMission_AgentId",
                table: "PerformingMission");

            migrationBuilder.DropIndex(
                name: "IX_PerformingMission_MissionId",
                table: "PerformingMission");

            migrationBuilder.CreateIndex(
                name: "IX_PerformingMission_AgentId",
                table: "PerformingMission",
                column: "AgentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerformingMission_MissionId",
                table: "PerformingMission",
                column: "MissionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PerformingMission_AgentId",
                table: "PerformingMission");

            migrationBuilder.DropIndex(
                name: "IX_PerformingMission_MissionId",
                table: "PerformingMission");

            migrationBuilder.CreateIndex(
                name: "IX_PerformingMission_AgentId",
                table: "PerformingMission",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformingMission_MissionId",
                table: "PerformingMission",
                column: "MissionId");
        }
    }
}
