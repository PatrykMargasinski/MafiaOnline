using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AdditionalMissionTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MissionTemplate",
                columns: new[] { "Id", "DexterityPercentage", "IntelligencePercentage", "MaxDifficulty", "MaxDuration", "MaxLoot", "MinDifficulty", "MinDuration", "MinLoot", "Name", "StrengthPercentage" },
                values: new object[,]
                {
                    { 7L, (short)10, (short)10, (short)7, 30, 2000, (short)3, 10, 2000, "Vandalism", (short)80 },
                    { 8L, (short)80, (short)10, (short)7, 30, 5000, (short)3, 10, 2000, "Arson on the building", (short)10 },
                    { 9L, (short)10, (short)80, (short)7, 30, 5000, (short)3, 10, 2000, "Gambling manilupation", (short)10 },
                    { 10L, (short)30, (short)30, (short)6, 30, 5000, (short)4, 10, 3000, "Blackmail", (short)40 },
                    { 11L, (short)40, (short)30, (short)6, 30, 5000, (short)4, 10, 3000, "Drug smuggling", (short)30 },
                    { 12L, (short)30, (short)40, (short)7, 30, 5000, (short)6, 10, 3000, "Deal you can not throw away", (short)30 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MissionTemplate",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "MissionTemplate",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "MissionTemplate",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "MissionTemplate",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "MissionTemplate",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "MissionTemplate",
                keyColumn: "Id",
                keyValue: 12L);
        }
    }
}
