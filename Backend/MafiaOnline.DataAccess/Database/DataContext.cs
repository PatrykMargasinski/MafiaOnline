using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace MafiaOnline.DataAccess.Database
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions options) : base(options)
        {

        }


        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<Boss> Bosses { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Mission> Missions { get; set; }
        public virtual DbSet<MovingAgent> MovingAgents { get; set; }
        public virtual DbSet<MissionTemplate> MissionTemplates { get; set; }
        public virtual DbSet<PerformingMission> PerformingMissions { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<AgentForSale> AgentsForSale { get; set; }
        public virtual DbSet<Name> Names { get; set; }
        public virtual DbSet<MapElement> MapElements { get; set; }
        public virtual DbSet<ExposedMapElement> ExposedMapElements { get; set; }
        public virtual DbSet<Headquarters> Headquarters { get; set; }
        public virtual DbSet<Ambush> Ambushes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}
