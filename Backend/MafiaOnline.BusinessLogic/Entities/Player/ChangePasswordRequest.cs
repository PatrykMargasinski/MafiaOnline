﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class ChangePasswordRequest
    {
        public long PlayerId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatedNewPassword { get; set; }
    }
}
