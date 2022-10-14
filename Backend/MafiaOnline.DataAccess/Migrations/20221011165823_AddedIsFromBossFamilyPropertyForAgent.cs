using Microsoft.EntityFrameworkCore.Migrations;

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedIsFromBossFamilyPropertyForAgent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFromBossFamily",
                table: "Agent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Agent",
                columns: new[] { "Id", "BossId", "Dexterity", "FirstName", "Intelligence", "IsFromBossFamily", "LastName", "State", "Strength", "Upkeep" },
                values: new object[] { 6L, 1L, 10, "Ricardo", 10, true, "Rico", 2, 10, 100L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DropColumn(
                name: "IsFromBossFamily",
                table: "Agent");
        }
    }
}
