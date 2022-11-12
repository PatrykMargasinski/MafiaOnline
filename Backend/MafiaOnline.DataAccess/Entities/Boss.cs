using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;


namespace MafiaOnline.DataAccess.Entities
{
    public partial class Boss : Entity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public long Money { get; set; }
        public DateTime? LastSeen { get; set; }

        public virtual Player Player { get; set; }
        public virtual List<Agent> Agents { get; set; }
        public virtual List<Message> MessageFromBosses { get; set; }
        public virtual List<Message> MessageToBosses { get; set; }

        public virtual Headquarters Headquarters { get; set; }
    }

    public class BossModelConfiguration : IEntityTypeConfiguration<Boss>
    {
        public void Configure(EntityTypeBuilder<Boss> builder)
        {
            builder.ToTable("Boss");
        }
    }
}
