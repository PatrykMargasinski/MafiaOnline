using Microsoft.EntityFrameworkCore;
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

        public virtual string Description
        {
            get { return null; }
        }

        public virtual long? Owner
        {
            get { return null; }
        }
    }

    public enum MapElementType
    {
        None,
        Headquarters
    }

    public class MapElementModelConfiguration : IEntityTypeConfiguration<MapElement>
    {
        public void Configure(EntityTypeBuilder<MapElement> builder)
        {
            builder.ToTable("MapElement");

            builder.Property(x => x.Type).HasConversion<int>();
        }
    }
}
