using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class Ambush : Entity
    {
        public long AgentId { get; set; }
        public long MapElementId { get; set; }
        public long BossId { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual MapElement MapElement { get; set; }
        public virtual Boss Boss{ get; set; }
    }

    public class AmbushModelConfiguration : IEntityTypeConfiguration<Ambush>
    {
        public void Configure(EntityTypeBuilder<Ambush> builder)
        {
            builder.ToTable("Ambush");

            builder.HasOne(d => d.Agent)
                .WithOne(p => p.Ambush)
                .HasForeignKey<Ambush>(d => d.AgentId);

            builder.HasOne(d => d.Boss)
                .WithMany(p => p.Ambushes)
                .HasForeignKey(d => d.BossId);
        }
    }
}
