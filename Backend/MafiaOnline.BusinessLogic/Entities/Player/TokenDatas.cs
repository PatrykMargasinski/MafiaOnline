using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class TokenDatas
    {
        public long PlayerId { get; set; }
        public long BossId { get; set; }
        public string PlayerNick { get; set; }
        public string PlayerRole { get; set; }
    }
}
