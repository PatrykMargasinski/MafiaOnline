using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class BossRankingView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "CREATE VIEW VBossRanking AS " +
                "SELECT " +
                "ROW_NUMBER() OVER( ORDER BY Money DESC) AS Lp," +
                "Id," +
                "FirstName," +
                "LastName," +
                "Money," +
                "DENSE_RANK() OVER( ORDER BY Money DESC) AS Position " +
                "FROM Boss " +
                "ORDER BY Money DESC OFFSET 0 ROWS"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DROP VIEW VBossRanking"
            );
        }
    }
}
