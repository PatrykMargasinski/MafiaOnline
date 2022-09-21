using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class BasicModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boss",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Money = table.Column<long>(type: "bigint", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false),
                    StrengthPercentage = table.Column<int>(type: "int", nullable: false),
                    DexterityPercentage = table.Column<int>(type: "int", nullable: false),
                    IntelligencePercentage = table.Column<int>(type: "int", nullable: false),
                    Loot = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BossId = table.Column<long>(type: "bigint", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Dexterity = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Upkeep = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agent_Boss_BossId",
                        column: x => x.BossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToBossId = table.Column<long>(type: "bigint", nullable: true),
                    FromBossId = table.Column<long>(type: "bigint", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Boss_FromBossId",
                        column: x => x.FromBossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Boss_ToBossId",
                        column: x => x.ToBossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nick = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BossId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Boss_BossId",
                        column: x => x.BossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgentForSale",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentForSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentForSale_Agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerformingMission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MissionId = table.Column<long>(type: "bigint", nullable: false),
                    AgentId = table.Column<long>(type: "bigint", nullable: false),
                    CompletionTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformingMission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformingMission_Agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformingMission_Mission_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Mission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Agent",
                columns: new[] { "Id", "BossId", "Dexterity", "FirstName", "Intelligence", "LastName", "Strength", "Upkeep" },
                values: new object[] { 4L, null, 7, "Eleonora", 0, "Lora", 8, 30L });

            migrationBuilder.InsertData(
                table: "Boss",
                columns: new[] { "Id", "FirstName", "LastName", "LastSeen", "Money" },
                values: new object[,]
                {
                    { 1L, "Patricio", "Rico", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000L },
                    { 2L, "Rodrigo", "Margherita", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000L }
                });

            migrationBuilder.InsertData(
                table: "Mission",
                columns: new[] { "Id", "DexterityPercentage", "DifficultyLevel", "Duration", "IntelligencePercentage", "Loot", "Name", "StrengthPercentage" },
                values: new object[,]
                {
                    { 1L, 60, 7, 30.0, 20, 5000, "Bank robbery", 20 },
                    { 2L, 20, 9, 10.0, 0, 10000, "Senator assassination", 80 },
                    { 3L, 20, 2, 10.0, 20, 100, "Party", 60 },
                    { 4L, 60, 1, 5.0, 20, 10, "Buy a coffee", 20 },
                    { 5L, 20, 5, 55.0, 60, 1000, "Money laundering", 20 },
                    { 6L, 60, 6, 3600.0, 20, 2000, "Car theft", 20 },
                    { 7L, 20, 8, 15.0, 40, 4000, "Arms trade", 40 }
                });

            migrationBuilder.InsertData(
                table: "Agent",
                columns: new[] { "Id", "BossId", "Dexterity", "FirstName", "Intelligence", "LastName", "Strength", "Upkeep" },
                values: new object[,]
                {
                    { 1L, 1L, 10, "Jotaro", 10, "Kujo", 10, 100L },
                    { 2L, 1L, 5, "Adam", 5, "Mickiewicz", 5, 50L },
                    { 5L, 1L, 1, "Robert", 5, "Makłowicz", 3, 200L },
                    { 3L, 2L, 4, "Natalia", 3, "Natsu", 7, 70L }
                });

            migrationBuilder.InsertData(
                table: "AgentForSale",
                columns: new[] { "Id", "AgentId", "Price" },
                values: new object[] { 1L, 4L, 5000L });

            migrationBuilder.InsertData(
                table: "Player",
                columns: new[] { "Id", "BossId", "Nick", "Password" },
                values: new object[,]
                {
                    { 1L, 1L, "mafia", "tlnK6HiwFF4+b4DRVaVdRlIPtzduirsf8W3+nbXlLWlf9c/J" },
                    { 2L, 2L, "tomek", "d2JZt0Jz9UzgW1l544W2WnOaX14u/pfGUDYTQzv5AEWk3W7D" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agent_BossId",
                table: "Agent",
                column: "BossId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentForSale_AgentId",
                table: "AgentForSale",
                column: "AgentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromBossId",
                table: "Message",
                column: "FromBossId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ToBossId",
                table: "Message",
                column: "ToBossId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformingMission_AgentId",
                table: "PerformingMission",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformingMission_MissionId",
                table: "PerformingMission",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_BossId",
                table: "Player",
                column: "BossId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentForSale");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "PerformingMission");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropTable(
                name: "Mission");

            migrationBuilder.DropTable(
                name: "Boss");
        }
    }
}
