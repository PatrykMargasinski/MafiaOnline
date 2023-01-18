using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class AmbushingAgentDTO
    {
        public long Id { get; set; }
        public long AmbushId { get; set; }
        public long MapElementId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Strength { get; set; }
        public string Dexterity { get; set; }
        public string Intelligence { get; set; }
        public Point Position { get; set; }
    }
}
