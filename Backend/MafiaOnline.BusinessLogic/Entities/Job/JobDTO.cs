using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class JobDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ExecutionTime { get; set; }
    }
}
