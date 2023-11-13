using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class Agent : Entity
    {
        public Agent()
        {

        }
        public long? BossId { get; set; }
        [NotMapped]
        public AgentState? StateIdEnum 
        {
            get => (AgentState?) StateId;
            set => StateId = (long?) value;
        }

        public long? StateId { get; set; }

        [NotMapped]
        public AgentSubstate? SubstateIdEnum 
        {
            get => (AgentSubstate?) SubstateId;
            set => SubstateId = (long?) value;
        }

        public long? SubstateId { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (FirstName == null || LastName == null) return null;
                return $"{FirstName} {LastName}";
            }
        }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public long Upkeep { get; set; }
        public bool IsFromBossFamily { get; set; }

        public virtual Boss Boss { get; set; }
        public virtual Ambush Ambush { get; set; }
        public virtual AgentForSale AgentForSale { get; set; }
        public virtual MovingAgent MovingAgent { get; set; }
        public virtual PerformingMission PerformingMission { get; set; }

        public virtual State State { get; set; }

        public virtual Substate Substate { get; set; }
    }

    public enum AgentState
    {
        Renegate = 1,
        ForSale = 2,
        Active = 3,
        OnMission = 4,
        Moving = 5,
        Ambushing = 6
    }

    public enum AgentSubstate
    {
        MovingOnMission = 1,
        Patrolling = 2,
        MovingWithLoot = 3 //TODO: Dodać do seedera
    }

    public class AgentModelConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.ToTable("Agent");

            builder.HasOne(d => d.Boss)
                .WithMany(p => p.Agents)
                .HasForeignKey(d => d.BossId);

            builder.HasOne(a => a.State)
                .WithMany()
                .HasForeignKey(a => a.StateId);

            builder.HasOne(a => a.Substate)
                .WithMany()
                .HasForeignKey(a => a.SubstateId);
        }
    }
}
