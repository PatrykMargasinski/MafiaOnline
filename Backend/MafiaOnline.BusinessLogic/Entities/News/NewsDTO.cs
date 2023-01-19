using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class NewsDTO
    {
        public long Id { get; set;  }
        public string Subject { get; set; }
        public string HTMLContent { get; set; }
    }
}
