using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class ArrangeAmbushRequest
    {
        public long AgentId { get; set; }
        public long BossId { get; set; }
        public Point Point { get; set; }
    }
}
