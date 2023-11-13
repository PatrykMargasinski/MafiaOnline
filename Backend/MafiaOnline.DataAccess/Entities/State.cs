using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MafiaOnline.DataAccess.Entities
{
    public class State : Entity
    {
        public State()
        {

        }
        public string Name { get; set; }

        public virtual IList<Substate> Substates { get; set; }
    }

    public class StateModelConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("State");
        }
    }
}
