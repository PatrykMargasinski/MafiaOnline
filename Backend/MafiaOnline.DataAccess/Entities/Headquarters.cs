using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public class Headquarters : MapElement
    {
        public string Name { get; set; }
        public long BossId { get; set; }
        public virtual Boss Boss { get; set; }

        public override string Description
        {
            get { return Name; }
        }

        public override long? Owner
        {
            get { return BossId; }
        }
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
