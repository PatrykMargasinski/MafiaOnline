using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class DeleteAccountRequest
    {
        public long PlayerId { get; set; }
        public string Password { get; set; }
    }
}
