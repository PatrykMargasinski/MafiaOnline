using Microsoft.EntityFrameworkCore.Migrations;

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedMessageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Boss_ToBossId",
                table: "Message");

            migrationBuilder.AlterColumn<long>(
                name: "ToBossId",
                table: "Message",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Boss_ToBossId",
                table: "Message",
                column: "ToBossId",
                principalTable: "Boss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Boss_ToBossId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Message");

            migrationBuilder.AlterColumn<long>(
                name: "ToBossId",
                table: "Message",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Boss_ToBossId",
                table: "Message",
                column: "ToBossId",
                principalTable: "Boss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
