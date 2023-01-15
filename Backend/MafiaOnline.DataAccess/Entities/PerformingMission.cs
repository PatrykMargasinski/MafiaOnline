using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class PerformingMission : Entity
    {
        public long MissionId { get; set; }
        public long AgentId { get; set; }
        public DateTime CompletionTime { get; set; }
        [NotMapped]
        public Point[] WayBack
        {
            get
            {
                if (string.IsNullOrEmpty(WayBackJson)) return null;
                return JsonSerializer.Deserialize<Point[]>(WayBackJson);
            }
            set
            {
                WayBackJson = value == null ? null : JsonSerializer.Serialize(value);
            }
        }
        public string WayBackJson { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual Mission Mission { get; set; }
    }

    public class PerformingMissionModelConfiguration : IEntityTypeConfiguration<PerformingMission>
    {
        public void Configure(EntityTypeBuilder<PerformingMission> builder)
        {
            builder.ToTable("PerformingMission");

            builder.HasOne(d => d.Agent)
                .WithOne(p => p.PerformingMission)
                .HasForeignKey<PerformingMission>(d => d.AgentId);

            builder.HasOne(d => d.Mission)
                .WithOne(p => p.PerformingMission)
                .HasForeignKey<PerformingMission>(d => d.MissionId);
        }
    }
}
