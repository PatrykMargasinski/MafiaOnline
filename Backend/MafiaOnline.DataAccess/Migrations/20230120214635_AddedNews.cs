using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HTMLContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "HTMLContent", "Priority", "Subject" },
                values: new object[] { 1L, "We are glad that version 2.0 has just seen the light of day. There are many features the new version has provided. <br>The most important of them are:<ul><li>Map - all game events are already taking place on the map</li><li>3 important map elements have been introduced: heaquarters, missions, ambushes</li><li>Every boss has own headquarters. All the agents leave right here. They also come back here.<li>Missions are map elements - the agent must reach the mission to perform it. </li><li>Ambushes are another map elements. When agent moves on a map he may fall into a ambush along the way.</li></ul>Every new feature is described in the \"About\" section", (short)5, "Version 2.0 out!" });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "HTMLContent", "Priority", "Subject" },
                values: new object[] { 2L, "Now we can inform you about some interesting news related to the game", (short)1, "News added" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
