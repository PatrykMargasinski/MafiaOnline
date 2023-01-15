﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class Agent : Entity
    {
        public long? BossId { get; set; }
        public AgentState State { get; set; }
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
    }

    public enum AgentState
    {
        Renegate,
        ForSale,
        Active,
        OnMission,
        Moving,
        MovingWithLoot,
        Ambushing
    }

    public class AgentModelConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.ToTable("Agent");

            builder.Property(x => x.State).HasConversion<int>();

            builder.HasOne(d => d.Boss)
                .WithMany(p => p.Agents)
                .HasForeignKey(d => d.BossId);
        }
    }
}
