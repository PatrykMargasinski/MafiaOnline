﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class DismissAgentRequest
    {
        public long AgentId { get; set; }
        public long BossId { get; set; }
    }
}
