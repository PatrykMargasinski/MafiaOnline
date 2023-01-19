using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "Player",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1L, "Administrator" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2L, "Player" });

            migrationBuilder.UpdateData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RoleId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RoleId",
                value: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_Player_RoleId",
                table: "Player",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Role_RoleId",
                table: "Player",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Role_RoleId",
                table: "Player");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Player_RoleId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Player");
        }
    }
}
