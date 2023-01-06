using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class MovingAgentDTO
    {
        public long Id { get; set; }
        public long? BossId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public Point CurrentPosition { get; set; }
        public Point DestinationPosition { get; set; }
        public string DestinationDescription{ get; set; }
    }
}
