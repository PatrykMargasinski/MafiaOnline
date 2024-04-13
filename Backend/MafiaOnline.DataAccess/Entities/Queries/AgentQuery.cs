using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Entities.Queries
{
    public class AgentQuery : BaseQuery<VAgent>
    {
        public string Name { get; set; }
        public AgentState? State { get; set; }
    }
}
