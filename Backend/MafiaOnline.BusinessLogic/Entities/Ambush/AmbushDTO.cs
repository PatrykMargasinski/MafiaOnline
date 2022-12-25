using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class AmbushDTO
    {
        public long AgentId { get; set; }
        public long BossId { get; set; }
        public long MapElementId { get; set; }
        public string AgentFullName { get; set; }
        public string BossLastName { get; set; }
    }
}
