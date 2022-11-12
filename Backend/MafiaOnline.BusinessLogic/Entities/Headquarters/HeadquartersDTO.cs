using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class HeadquartersDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string BossFirstName { get; set; }
        public string BossLastName { get; set; }
    }
}
