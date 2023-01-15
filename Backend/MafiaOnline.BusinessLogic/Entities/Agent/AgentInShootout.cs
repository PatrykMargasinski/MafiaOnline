using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class AgentInShootout
    {
        public long Id { get; set; }
        public int[] Attributes { get; set; }
        public string FullName { get; set; }
        public int Power { get; set; }
        public short Points { get; set; }
    }
}
