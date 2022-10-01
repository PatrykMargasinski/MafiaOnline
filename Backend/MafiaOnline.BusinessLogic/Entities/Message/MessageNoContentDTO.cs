using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class MessageNoContentDTO
    {
        public long ToBossId { get; set; }
        public long? FromBossId { get; set; }
        public string ToBossName { get; set; }
        public string FromBossName { get; set; }
        public string Subject { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool Seen { get; set; }
    }
}
