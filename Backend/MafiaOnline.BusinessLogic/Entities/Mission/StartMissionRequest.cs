using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities.Mission
{
    public class StartMissionRequest
    {
        public long AgentId { get; set; }
        public long MissionId { get; set; }
    }
}
