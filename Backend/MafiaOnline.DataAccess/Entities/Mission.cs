﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;


namespace MafiaOnline.DataAccess.Entities
{
    public class Mission : Entity
    {
        public string Name { get; set; }
        public int DifficultyLevel { get; set; }
        public int StrengthPercentage { get; set; }
        public int DexterityPercentage { get; set; }
        public int IntelligencePercentage { get; set; }
        public int Loot { get; set; }
        public double Duration { get; set; }
        public virtual List<PerformingMission> PerformingMissions { get; set; }
    }

    public class MissionModelConfiguration : IEntityTypeConfiguration<Mission>
    {
        public void Configure(EntityTypeBuilder<Mission> builder)
        {
            builder.ToTable("Mission");
        }
    }
}
