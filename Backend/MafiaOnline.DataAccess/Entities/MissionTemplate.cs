using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;


namespace MafiaOnline.DataAccess.Entities
{
    public class MissionTemplate : Entity
    {
        public string Name { get; set; }
        public short MinDifficulty { get; set; }
        public short MaxDifficulty { get; set; }
        public int MinLoot { get; set; }
        public int MaxLoot { get; set; }
        public int MinDuration { get; set; }
        public int MaxDuration { get; set; }
        public short StrengthPercentage { get; set; }
        public short DexterityPercentage { get; set; }
        public short IntelligencePercentage { get; set; }
    }

    public class MissionTemplateModelConfiguration : IEntityTypeConfiguration<MissionTemplate>
    {
        public void Configure(EntityTypeBuilder<MissionTemplate> builder)
        {
            builder.ToTable("MissionTemplate");
        }
    }
}
