using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class MapElementDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MapElement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-1234-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "bcbd1886-48ba-4279-aedf-74d13e5075ee");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "c2cb85e3-a526-4e44-86c5-eb54dd29081d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-1234-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7ec36018-7b22-493f-b905-ed14b3b967db", "AQAAAAEAACcQAAAAEPwGzPPEhg0relp2mW/R1MkIA5c3Sk7J8/DEBqBGC9Uhu/PDj/6HGlz6JpWfwUxKow==", "72b28cc9-bf7d-4bc4-9844-65aa726d173c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b4115d06-c977-4c66-8a2a-a187998a0edd", "AQAAAAEAACcQAAAAEE1WkiE9/Jb8XwH+aj1KZpdF2P3KAnk8txSFfT5HUz/BtfiigAE7IRaXFndpFDytlw==", "ff11d3a0-2ed7-42e2-bef1-e53ddf29f016" });

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Description",
                value: "Headquarters of Patricio Rico");

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Description",
                value: "Headquarters of Rodrigo Margherita");

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Description",
                value: "Mission: Bank robbery");

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Description",
                value: "Mission: Senator assassination");

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Description",
                value: "Mission: Party");

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Description",
                value: "Mission: Buy a coffee");

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Description",
                value: "Mission: Money laundering");

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Description",
                value: "Mission: Car theft");

            migrationBuilder.UpdateData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Description",
                value: "Mission: Arms trade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MapElement");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-1234-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "0b8ec34a-fc53-46cb-8716-d38273d8f84a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "69199552-dc2b-45b1-83b0-dc8a10022532");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-1234-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "10e79cc2-9e44-4a6f-a6d8-f242b0cd8d6d", "AQAAAAEAACcQAAAAEBVmXM1OCPA8CbvPA+SDaISAbx193z5ePYcBpmBfl1zFWyy1/maU+6jPbJk92wn90g==", "81420fb9-7766-4e69-90fc-193f89c86248" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "889f886a-cf28-437a-a4de-7d0d498f700c", "AQAAAAEAACcQAAAAEM7lbSTMvQtfd9mSSxISCXOlc686Rf9ePhRlCqJ85NJKNp60TBUfA7wYkVQq8s+IVw==", "4656772e-0687-41dd-a2f1-9465e363d3b9" });
        }
    }
}
