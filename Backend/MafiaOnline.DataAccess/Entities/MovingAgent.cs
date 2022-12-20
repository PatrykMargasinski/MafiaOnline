using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Xml.Linq;

namespace MafiaOnline.DataAccess.Entities
{
    public class MovingAgent : Entity
    {
        public long AgentId { get; set; }
        [NotMapped]
        public Point[] Path
        {
            get
            {
                if (string.IsNullOrEmpty(PathJson)) return null;
                return JsonSerializer.Deserialize<Point[]>(PathJson);
            }
            set
            {
                PathJson = value == null ? null : JsonSerializer.Serialize(value);
            }
        }
        public string PathJson { get; set; }
        public long? Step { get; set; }
        public DateTime? ConstCompletionTime { get; set; }
        [NotMapped]
        public Point DestinationPoint
        {
            get
            {
                if (string.IsNullOrEmpty(DestinationJson)) return null;
                return JsonSerializer.Deserialize<Point>(DestinationJson);
            }
            set
            {
                DestinationJson = value == null ? null : JsonSerializer.Serialize(value);
            }
        }
        public string DestinationJson { get; set; }
        public string DestinationDescription { get; set; }
        public string DatasJson { get; set; }
        public string JobKey { get; set; }
        public virtual Agent Agent { get; set; }
    }

    public class MovingAgentModelConfiguration : IEntityTypeConfiguration<MovingAgent>
    {
        public void Configure(EntityTypeBuilder<MovingAgent> builder)
        {
            builder.ToTable("MovingAgent");

            builder.HasOne(d => d.Agent)
                .WithOne(x => x.MovingAgent)
                .HasForeignKey<MovingAgent>(x => x.AgentId);
        }
    }

    public class Point
    {
        public long X { get; set; }
        public long Y { get; set; }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object other)
        {
            return X == ((Point)other).X && Y == ((Point)other).Y;
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() ^ Y.GetHashCode());
        }
    }

}
