using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedMissionTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MissionTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinDifficulty = table.Column<short>(type: "smallint", nullable: false),
                    MaxDifficulty = table.Column<short>(type: "smallint", nullable: false),
                    MinLoot = table.Column<int>(type: "int", nullable: false),
                    MaxLoot = table.Column<int>(type: "int", nullable: false),
                    MinDuration = table.Column<int>(type: "int", nullable: false),
                    MaxDuration = table.Column<int>(type: "int", nullable: false),
                    StrengthPercentage = table.Column<short>(type: "smallint", nullable: false),
                    DexterityPercentage = table.Column<short>(type: "smallint", nullable: false),
                    IntelligencePercentage = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionTemplate", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MissionTemplate",
                columns: new[] { "Id", "DexterityPercentage", "IntelligencePercentage", "MaxDifficulty", "MaxDuration", "MaxLoot", "MinDifficulty", "MinDuration", "MinLoot", "Name", "StrengthPercentage" },
                values: new object[,]
                {
                    { 1L, (short)20, (short)20, (short)8, 30, 5000, (short)4, 10, 1000, "Assassination", (short)60 },
                    { 2L, (short)60, (short)20, (short)8, 30, 5000, (short)4, 10, 1000, "Theft", (short)20 },
                    { 3L, (short)20, (short)60, (short)8, 30, 5000, (short)4, 10, 1000, "Money laundering", (short)20 },
                    { 4L, (short)20, (short)20, (short)10, 30, 10000, (short)6, 10, 4000, "Bank robbery", (short)60 },
                    { 5L, (short)60, (short)20, (short)10, 30, 10000, (short)6, 10, 4000, "Drug production", (short)20 },
                    { 6L, (short)20, (short)60, (short)10, 30, 10000, (short)6, 10, 4000, "Arms trade", (short)20 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionTemplate");
        }
    }
}
