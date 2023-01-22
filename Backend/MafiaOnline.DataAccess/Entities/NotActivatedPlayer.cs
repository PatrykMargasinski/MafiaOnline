using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class NotActivatedPlayer : Entity
    {
        public long PlayerId { get; set; }
        virtual public Player Player { get; set; }
        public string ActivationCode { get; set; }
        public DateTime DateOfDeletion { get; set; }
        public string JobKey { get; set; }
    }

    public class NotActivatedPlayerModelConfiguration : IEntityTypeConfiguration<NotActivatedPlayer>
    {
        public void Configure(EntityTypeBuilder<NotActivatedPlayer> builder)
        {
            builder.ToTable("NotActivatedPlayer");

            builder.HasOne(d => d.Player)
                .WithOne(d => d.NotActivatedPlayer)
                .HasForeignKey<NotActivatedPlayer>(d => d.PlayerId);
        }
    }
}
