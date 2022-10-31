using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class PerformingMissionDTO
    {
        public long Id { get; set;  }
        public long MissionId { get; set; }
        public long AgentId { get; set; }
        public string MissionName { get; set; }
        public string AgentName { get; set; }
        public int SuccessChance { get; set; }
        public int Loot { get; set; }
        public DateTime CompletionTime { get; set; }
        public long SecondsLeft { get; set; }
    }
}
