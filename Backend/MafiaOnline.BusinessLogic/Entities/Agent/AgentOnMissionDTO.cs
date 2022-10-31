using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class AgentOnMissionDTO
    {
        public long Id { get; set; }
        public long PerformingMissionId { get; set; }
        public long MissionId { get; set; }
        public string AgentName { get; set; }
        public string MissionName { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int SuccessChance { get; set; }
        public DateTime CompletionTime { get; set; }
        public long SecondsLeft { get; set; }
    }
}
