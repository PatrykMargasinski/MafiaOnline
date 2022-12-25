using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class Boss : Entity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (FirstName == null || LastName == null) return null;
                return $"{FirstName} {LastName}";
            }
        }
        public long Money { get; set; }
        public DateTime? LastSeen { get; set; }

        public virtual Player Player { get; set; }
        public virtual List<Agent> Agents { get; set; }
        public virtual List<Message> MessageFromBosses { get; set; }
        public virtual List<Message> MessageToBosses { get; set; }
        public virtual Headquarters Headquarters { get; set; }
        public virtual List<MapElement> MapElements { get; set; }
        public virtual List<Ambush> Ambushes { get; set; }
    }

    public class BossModelConfiguration : IEntityTypeConfiguration<Boss>
    {
        public void Configure(EntityTypeBuilder<Boss> builder)
        {
            builder.ToTable("Boss");
        }
    }
}
