using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class VAgent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sqlFilePath = Path.Combine("..", "MafiaOnline.DataAccess", "Migrations", "Sqls", "CreateVAgent.sql");

            string sqlScript = File.ReadAllText(sqlFilePath);

            migrationBuilder.Sql(sqlScript);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS VAgent");
        }
    }
}
