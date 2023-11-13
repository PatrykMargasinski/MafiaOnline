using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public class Mission : Entity
    {
        public long MapElementId { get; set; }
        public string Name { get; set; }
        public int DifficultyLevel { get; set; }
        public int StrengthPercentage { get; set; }
        public int DexterityPercentage { get; set; }
        public int IntelligencePercentage { get; set; }
        public int Loot { get; set; }
        public double Duration { get; set; }
        public bool RepeatableMission { get; set; }

        [NotMapped]
        public MissionState? StateIdEnum
        {
            get => (MissionState?) StateId;
            set => StateId = (long?) value;
        }

        public long? StateId { get; set; }

        public virtual PerformingMission PerformingMission { get; set; }
        public virtual MapElement MapElement { get; set; }
        public virtual State State { get; set; }
    }

    public enum MissionState
    {
        Available = 7,
        Performing = 8
    }

    public class MissionModelConfiguration : IEntityTypeConfiguration<Mission>
    {
        public void Configure(EntityTypeBuilder<Mission> builder)
        {
            builder.ToTable("Mission");

            builder.HasOne(a => a.State)
                .WithMany()
                .HasForeignKey(a => a.StateId);
        }
    }
}
