using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedJobKeyForPerformingMission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobKey",
                table: "PerformingMission",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobKey",
                table: "PerformingMission");
        }
    }
}
