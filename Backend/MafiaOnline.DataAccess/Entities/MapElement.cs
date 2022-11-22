﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public class MapElement : Entity
    {
        public long X { get; set; }
        public long Y { get; set; }
        public MapElementType Type { get; set; }
        public long? BossId { get; set; }
        public virtual Boss Boss { get; set; }
        public virtual Headquarters Headquarters { get; set; }
        public virtual Mission Mission { get; set; }
    }

    public enum MapElementType
    {
        None,
        Headquarters,
        Mission
    }

    public class MapElementModelConfiguration : IEntityTypeConfiguration<MapElement>
    {
        public void Configure(EntityTypeBuilder<MapElement> builder)
        {
            builder.ToTable("MapElement");

            builder.Property(x => x.Type).HasConversion<int>();

            builder.HasOne(d => d.Boss)
               .WithMany(d => d.MapElements)
               .HasForeignKey(d => d.BossId);

            builder.HasOne(d => d.Headquarters)
                .WithOne(d => d.MapElement)
                .HasForeignKey<Headquarters>(d => d.MapElementId);

            builder.HasOne(d => d.Mission)
                .WithOne(d => d.MapElement)
                .HasForeignKey<Mission>(d => d.MapElementId);
        }
    }
}
