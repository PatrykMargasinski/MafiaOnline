using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedStartOfSalePropertyForAgentForSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AgentForSale",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartOfSale",
                table: "AgentForSale",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 4L,
                column: "State",
                value: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartOfSale",
                table: "AgentForSale");

            migrationBuilder.UpdateData(
                table: "Agent",
                keyColumn: "Id",
                keyValue: 4L,
                column: "State",
                value: 1);

            migrationBuilder.InsertData(
                table: "AgentForSale",
                columns: new[] { "Id", "AgentId", "Price" },
                values: new object[] { 1L, 4L, 5000L });
        }
    }
}
