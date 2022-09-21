﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class PerformingMission : Entity
    {
        public long MissionId { get; set; }
        public long AgentId { get; set; }
        public DateTime CompletionTime { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual Mission Mission { get; set; }
    }

    public class PerformingMissionModelConfiguration : IEntityTypeConfiguration<PerformingMission>
    {
        public void Configure(EntityTypeBuilder<PerformingMission> builder)
        {
            builder.ToTable("PerformingMission");

            builder.HasOne(d => d.Agent)
                .WithMany(p => p.PerformingMissions)
                .HasForeignKey(d => d.AgentId);

            builder.HasOne(d => d.Mission)
                .WithMany(p => p.PerformingMissions)
                .HasForeignKey(d => d.MissionId);
        }
    }
}
