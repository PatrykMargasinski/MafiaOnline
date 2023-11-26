using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class StatesAndSubstatesImprovements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSubstates",
                table: "State",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 1L,
                column: "StateId",
                value: 4L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 2L,
                column: "StateId",
                value: 4L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 3L,
                column: "StateId",
                value: 4L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 5L,
                column: "StateId",
                value: 4L);

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 6L,
                column: "StateId",
                value: 4L);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-1234-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "b35a8440-8a82-472f-af19-b0d4297f28d1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "340a24cf-4d75-4cfe-acf9-f3aac620fc9c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-1234-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "683ed198-e5ea-4dd4-99cf-fcfce5dab672", "AQAAAAEAACcQAAAAEIs+TS4UWUHE8o1w9hoKHNr8mCPJeR01EjMpdGVEb9n04LxXhRPaAo78MY4JigGQag==", "c8669b52-83e5-4e6b-92f9-4cda0abd2be8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eac392b4-0be9-40a4-8894-c0cfa393b4c4", "AQAAAAEAACcQAAAAEA5uBw9OICPohpwkyXosEuLDLIY3IPwftbwvIs6rZssbAgzIvtoMCXEPJ24WboPPNw==", "239c464e-9a36-4140-af70-d1576faf1e03" });

            migrationBuilder.UpdateData(
                table: "State",
                keyColumn: "Id",
                keyValue: 5L,
                column: "HasSubstates",
                value: true);

            migrationBuilder.UpdateData(
                table: "Substate",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Patrolling");

            migrationBuilder.InsertData(
                table: "Substate",
                columns: new[] { "Id", "Name", "StateId" },
                values: new object[] { 3L, "MovingWithLoot", 5L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Substate",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DropColumn(
                name: "HasSubstates",
                table: "State");

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

            migrationBuilder.UpdateData(
                table: "Substate",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Performing");
        }
    }
}
