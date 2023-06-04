using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class CreateResetPasswordCodeRequest
    {
        public string Email { get; set; }
        public string ApiUrl { get; set; }
    }
}
