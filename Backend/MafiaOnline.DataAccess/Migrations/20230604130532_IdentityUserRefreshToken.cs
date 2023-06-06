using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class IdentityUserRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Player",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Player",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-1234-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "0b8ec34a-fc53-46cb-8716-d38273d8f84a", "PLAYER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "69199552-dc2b-45b1-83b0-dc8a10022532", "ADMINISTRATOR" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Player");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-1234-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "5adfd20a-c652-451c-b81f-3c16acf808c0", null });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "e1c892a0-3950-4815-8a4c-8d370568d5da", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-1234-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8522431e-d4fe-45cc-b5da-66c7c37a651b", "AQAAAAEAACcQAAAAEIdBf7QUWei/fmcQJeOOQWKGqe4QBN+H1CROcQbJ+Uj7ZKBK+IWIZs4gU+V/FCyKnA==", "f2b60cd0-00f4-4ab5-bdb4-a24b1710b561" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "efaaa111-3b8b-4b52-b450-2021c17ecd76", "AQAAAAEAACcQAAAAEE2IvNj0/R4L5y5xcM3DXjxRk5Bd1bMN+GYiw7smE1nf65inp+vhDNtE+5GL5d4XuA==", "e9ba9d77-41f7-4a1d-b371-aa4216bcd89d" });
        }
    }
}
