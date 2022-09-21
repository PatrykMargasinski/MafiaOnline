using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public class AgentForSale : Entity
    {
        public long AgentId { get; set; }
        public long Price { get; set; }
        public virtual Agent Agent { get; set; }
    }

    public class AgentForSaleModelConfiguration : IEntityTypeConfiguration<AgentForSale>
    {
        public void Configure(EntityTypeBuilder<AgentForSale> builder)
        {
            builder.ToTable("AgentForSale");

            builder.HasOne(d => d.Agent)
                .WithOne(x => x.AgentForSale)
                .HasForeignKey<AgentForSale>(x => x.AgentId);
        }
    }
}
