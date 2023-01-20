using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class News : Entity
    {
        public string Subject { get; set; }
        public string HTMLContent { get; set; }
        public short Priority { get; set; }
    }

    public class NewsModelConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.ToTable("News");
        }
    }
}
