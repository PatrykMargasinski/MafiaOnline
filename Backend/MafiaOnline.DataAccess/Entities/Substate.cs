using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class Substate : Entity
    {
        public Substate()
        {

        }
        public string Name { get; set; }
        public long StateId { get; set; }

        public virtual State State { get; set; }
    }

    public class SubstateModelConfiguration : IEntityTypeConfiguration<Substate>
    {
        public void Configure(EntityTypeBuilder<Substate> builder)
        {
            builder.ToTable("Substate");

            builder.HasOne(x => x.State)
                .WithMany(x => x.Substates)
                .HasForeignKey(x => x.StateId);
        }
    }
}
