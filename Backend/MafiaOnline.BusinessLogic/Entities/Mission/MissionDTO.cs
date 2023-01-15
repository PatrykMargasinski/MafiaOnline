using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class MissionDTO
    {
        public long Id { get; set;  }
        public long X { get; set; }
        public long Y { get; set; }
        public string Name { get; set; }
        public int DifficultyLevel { get; set; }
        public int StrengthPercentage { get; set; }
        public int DexterityPercentage { get; set; }
        public int IntelligencePercentage { get; set; }
        public int Loot { get; set; }
        public double Duration { get; set; }
    }
}
