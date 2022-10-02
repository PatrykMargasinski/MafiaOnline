using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class BossDTO
    {
        public string Name { get; set; }
        public long Money { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
