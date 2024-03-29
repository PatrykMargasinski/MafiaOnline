﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class RegisterRequest
    {
        public string ApiUrl { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string BossFirstName { get; set; }
        public string BossLastName { get; set; }
        public string HeadquartersName { get; set; }
        public string[] AgentNames { get; set; }
    }
}
