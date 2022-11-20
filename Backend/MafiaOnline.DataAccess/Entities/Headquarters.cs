using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public class Headquarters : Entity
    {
        public long MapElementId { get; set; }
        public string Name { get; set; }
        public long BossId { get; set; }
        public virtual Boss Boss { get; set; }
        public virtual MapElement MapElement { get; set; }
    }

    public class HeadquarterModelConfiguration : IEntityTypeConfiguration<Headquarters>
    {
        public void Configure(EntityTypeBuilder<Headquarters> builder)
        {
            builder.ToTable("Headquarters");

            builder.HasOne(d => d.Boss)
                .WithOne(p => p.Headquarters)
                .HasForeignKey<Headquarters>(d => d.BossId);
        }
    }
}
