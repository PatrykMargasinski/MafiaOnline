using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class Message : Entity
    {
        public long ToBossId { get; set; }
        public long? FromBossId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool Seen { get; set; }
        public virtual Boss FromBoss { get; set; }
        public virtual Boss ToBoss { get; set; }
    }

    public class MessageModelConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");

            builder.HasOne(d => d.FromBoss)
                .WithMany(p => p.MessageFromBosses)
                .HasForeignKey(d => d.FromBossId);

            builder.HasOne(d => d.ToBoss)
                .WithMany(p => p.MessageToBosses)
                .HasForeignKey(d => d.ToBossId);
        }
    }
}
