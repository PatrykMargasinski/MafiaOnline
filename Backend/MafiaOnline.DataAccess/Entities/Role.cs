using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class Role : Entity
    {
        public string Name { get; set; }
        public virtual IList<Player> Players { get; set; }
    }

    //role consts
    public class RoleConsts
    {
        public const string Player = "Player";
        public const string Administrator = "Administrator";
    }

    public class RoleModelConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
        }
    }
}
