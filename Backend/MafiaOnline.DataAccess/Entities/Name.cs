using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{

    public partial class Name : Entity
    {
        public string Text { get; set; }
        public NameType Type { get; set; }
    }

    public enum NameType
    {
        FirstName,
        LastName
    }

    public class NameModelConfiguration : IEntityTypeConfiguration<Name>
    {
        public void Configure(EntityTypeBuilder<Name> builder)
        {
            builder.ToTable("Name");

            builder.Property(x => x.Type).HasConversion<int>();
        }
    }
}
