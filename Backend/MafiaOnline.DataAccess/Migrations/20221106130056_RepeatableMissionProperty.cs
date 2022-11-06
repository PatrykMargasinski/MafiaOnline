using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class RepeatableMissionProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RepeatableMission",
                table: "Mission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RepeatableMission",
                value: true);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RepeatableMission",
                value: true);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RepeatableMission",
                value: true);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 4L,
                column: "RepeatableMission",
                value: true);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 5L,
                column: "RepeatableMission",
                value: true);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 6L,
                column: "RepeatableMission",
                value: true);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 7L,
                column: "RepeatableMission",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepeatableMission",
                table: "Mission");
        }
    }
}
