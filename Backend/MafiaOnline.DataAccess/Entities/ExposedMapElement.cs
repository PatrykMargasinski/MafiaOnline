using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public class ExposedMapElement : Entity
    {
        public long MapElementId { get; set; } 
        public long ExposedByBossId { get; set; }
        public virtual MapElement MapElement { get; set; }
        public virtual Boss Boss { get; set; }
    }

    public class ExposedMapElementModelConfiguration : IEntityTypeConfiguration<ExposedMapElement>
    {
        public void Configure(EntityTypeBuilder<ExposedMapElement> builder)
        {
            builder.ToTable("ExposedMapElement");

            builder.HasOne(x => x.Boss)
                .WithMany(x => x.Exposures)
                .HasForeignKey(x => x.ExposedByBossId);
        }
    }
}
