using Microsoft.EntityFrameworkCore.Migrations;

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class ChangedPasswordToHashedPasswordInPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Player",
                newName: "HashedPassword");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Player",
                newName: "Password");
        }
    }
}
