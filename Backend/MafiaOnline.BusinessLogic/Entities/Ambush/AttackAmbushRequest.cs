using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class AttackAmbushRequest
    {
        public long AgentId { get; set; }
        public long MapElementId { get; set; }
    }
}
