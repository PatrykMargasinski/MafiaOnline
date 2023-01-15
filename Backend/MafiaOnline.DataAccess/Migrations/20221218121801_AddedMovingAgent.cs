using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedMovingAgent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovingAgent",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<long>(type: "bigint", nullable: false),
                    PathJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Step = table.Column<long>(type: "bigint", nullable: true),
                    ConstCompletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DestinationJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatasJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovingAgent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovingAgent_Agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovingAgent_AgentId",
                table: "MovingAgent",
                column: "AgentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovingAgent");
        }
    }
}
