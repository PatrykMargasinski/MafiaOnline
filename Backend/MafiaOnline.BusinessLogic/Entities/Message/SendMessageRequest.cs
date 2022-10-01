using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class SendMessageRequest
    {
        public long FromBossId { get; set; }
        public string ToBossFullName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
