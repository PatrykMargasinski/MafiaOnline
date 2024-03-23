using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

#nullable disable

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AgentStateUpdateTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "CREATE TRIGGER trg_AgentStateUpdated\n" + 
                "ON Agent AFTER UPDATE AS\n" +
                "BEGIN\n" +
                "     IF UPDATE(StateId)\n" +
                "     BEGIN\n" +
                "         UPDATE a\n" +
                "         SET a.SubstateId = NULL\n" +
                "         FROM Agent a\n" +
                "         INNER JOIN inserted i ON a.Id = i.Id\n" +
                "         INNER JOIN State s on s.Id = a.StateId\n" +
                "         WHERE s.HasSubstates = 0\n" +
                "     END\n" +
                "END"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DROP TRIGGER trg_AgentStateUpdated"
            );
        }
    }
}
