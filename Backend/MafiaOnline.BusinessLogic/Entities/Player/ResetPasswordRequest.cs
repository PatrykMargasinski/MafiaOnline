using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class ResetPasswordRequest
    {
        public long PlayerId { get;set;}
        public string Code { get;set;}
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }
    }
}
