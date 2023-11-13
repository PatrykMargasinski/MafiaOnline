using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class StatesAndSubstates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Mission");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Agent");

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "Mission",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "Agent",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubstateId",
                table: "Agent",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Substate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Substate_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-1234-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "d07a3401-b26b-4d2f-bc02-1b07f019c33a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "5aa1cce9-291f-4df7-9018-1fb2200d750d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-1234-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e227e6fb-9b93-4022-a3d0-da2150398884", "AQAAAAEAACcQAAAAELDcVwj9F+/s6KsK35OpUmBM7v8owHmPmMd9ye0jJVIbp97w6YuBAwPbeRXubqFckw==", "61d28b89-543e-4263-a334-114362b961a6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "164aaf9c-5110-408e-917e-83039a091784", "AQAAAAEAACcQAAAAELwfulWvzu1PcgYuTHviO+3nDcgebI2VR8/6ycuToysym38rz8Hm53de6yTGyiKqrA==", "fdc1075a-4a36-476b-bb57-3334bb43425d" });

            migrationBuilder.InsertData(
                table: "State",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Renegate" },
                    { 2L, "ForSale" },
                    { 3L, "OnMission" },
                    { 4L, "Active" },
                    { 5L, "Moving" },
                    { 6L, "Ambushing" },
                    { 7L, "Available" },
                    { 8L, "Performing" }
                });

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 1L,
                column: "StateId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 2L,
                column: "StateId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 3L,
                column: "StateId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 4L,
                column: "StateId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 5L,
                column: "StateId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 6L,
                column: "StateId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 1L,
                column: "StateId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 2L,
                column: "StateId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 3L,
                column: "StateId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 4L,
                column: "StateId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 5L,
                column: "StateId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 6L,
                column: "StateId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 7L,
                column: "StateId",
                value: 7L);

            migrationBuilder.InsertData(
                table: "Substate",
                columns: new[] { "Id", "Name", "StateId" },
                values: new object[,]
                {
                    { 1L, "MovingOnMission", 5L },
                    { 2L, "Performing", 5L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mission_StateId",
                table: "Mission",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Agent_StateId",
                table: "Agent",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Agent_SubstateId",
                table: "Agent",
                column: "SubstateId");

            migrationBuilder.CreateIndex(
                name: "IX_Substate_StateId",
                table: "Substate",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agent_State_StateId",
                table: "Agent",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agent_Substate_SubstateId",
                table: "Agent",
                column: "SubstateId",
                principalTable: "Substate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mission_State_StateId",
                table: "Mission",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agent_State_StateId",
                table: "Agent");

            migrationBuilder.DropForeignKey(
                name: "FK_Agent_Substate_SubstateId",
                table: "Agent");

            migrationBuilder.DropForeignKey(
                name: "FK_Mission_State_StateId",
                table: "Mission");

            migrationBuilder.DropTable(
                name: "Substate");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropIndex(
                name: "IX_Mission_StateId",
                table: "Mission");

            migrationBuilder.DropIndex(
                name: "IX_Agent_StateId",
                table: "Agent");

            migrationBuilder.DropIndex(
                name: "IX_Agent_SubstateId",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Mission");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "SubstateId",
                table: "Agent");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Mission",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Agent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 1L,
                column: "State",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 2L,
                column: "State",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 3L,
                column: "State",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 5L,
                column: "State",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 6L,
                column: "State",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-1234-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "ce0ea5f2-edd4-420c-86f6-4c7fd34c8be5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "727c60fd-4e1d-4b42-a957-d82691652584");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-1234-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "86c6bee6-cfd0-44a4-bb08-2e204cfeee6c", "AQAAAAEAACcQAAAAEG1cgWlqxNe+64MP20i95P/QhjWXfHucmknFGZlJgxLV59PdlkKiwo/rVDwHHJ6vag==", "1c5671a7-2b10-47d6-a88c-5dd5be3c253d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1483bed8-a6ab-4ed1-9ec4-15d53ae0b581", "AQAAAAEAACcQAAAAEKCornzmDIMLygtZUIzi3oqqQkgGMyXAEX2g6F0ah22RnTGST2FQ0zetohNpRm9Vvg==", "ee1b2038-ee10-492a-862e-402c18a3f71b" });
        }
    }
}
