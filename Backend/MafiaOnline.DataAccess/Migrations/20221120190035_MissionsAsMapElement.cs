using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class MissionsAsMapElement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MapElementId",
                table: "Mission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "MapElement",
                columns: new[] { "Id", "BossId", "Type", "X", "Y" },
                values: new object[,]
                {
                    { 3L, null, 2, 1L, 3L },
                    { 4L, null, 2, 3L, 5L },
                    { 5L, null, 2, 3L, 7L },
                    { 6L, null, 2, 5L, 3L },
                    { 7L, null, 2, 14L, 1L },
                    { 8L, null, 2, 13L, 3L },
                    { 9L, null, 2, 14L, 5L }
                });

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 1L,
                column: "MapElementId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 2L,
                column: "MapElementId",
                value: 4L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 3L,
                column: "MapElementId",
                value: 5L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 4L,
                column: "MapElementId",
                value: 6L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 5L,
                column: "MapElementId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 6L,
                column: "MapElementId",
                value: 8L);

            migrationBuilder.UpdateData(
                table: "Mission",
                keyColumn: "Id",
                keyValue: 7L,
                column: "MapElementId",
                value: 9L);

            migrationBuilder.CreateIndex(
                name: "IX_Mission_MapElementId",
                table: "Mission",
                column: "MapElementId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mission_MapElement_MapElementId",
                table: "Mission",
                column: "MapElementId",
                principalTable: "MapElement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mission_MapElement_MapElementId",
                table: "Mission");

            migrationBuilder.DropIndex(
                name: "IX_Mission_MapElementId",
                table: "Mission");

            migrationBuilder.DeleteData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "MapElement",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DropColumn(
                name: "MapElementId",
                table: "Mission");
        }
    }
}
