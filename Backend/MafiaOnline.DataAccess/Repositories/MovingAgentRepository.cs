using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface IMovingAgentRepository : ICrudRepository<MovingAgent>
    {
        string GetDatas(long movingAgentId);
    }

    public class MovingAgentRepository : CrudRepository<MovingAgent>, IMovingAgentRepository
    {
        public MovingAgentRepository(DataContext context) : base(context)
        {
            
        }

        public string GetDatas(long movingAgentId)
        {
            var movingAgent = _context.MovingAgents.FirstOrDefault(x => x.Id == movingAgentId);
            if (movingAgent == null)
                return null;
            return movingAgent.DatasJson;
        }
    }
}
