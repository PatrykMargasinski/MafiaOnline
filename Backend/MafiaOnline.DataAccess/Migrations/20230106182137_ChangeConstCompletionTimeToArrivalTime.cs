using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class ChangeConstCompletionTimeToArrivalTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstCompletionTime",
                table: "MovingAgent");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "MovingAgent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "MovingAgent");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConstCompletionTime",
                table: "MovingAgent",
                type: "datetime2",
                nullable: true);
        }
    }
}
