using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class Player : Entity
    {
        public string Nick { get; set; }
        public string HashedPassword { get; set; }
        public long BossId { get; set; }
        public virtual Boss Boss { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }

    public class PlayerModelConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Player");

            builder.HasOne(d => d.Boss)
                .WithOne(p => p.Player)
                .HasForeignKey<Player>(d => d.BossId);
        }
    }
}
