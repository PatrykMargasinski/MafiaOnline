using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface IAgentRepository : ICrudRepository<Agent>
    {

    }

    public class AgentRepository : CrudRepository<Agent>, IAgentRepository
    {
        public AgentRepository(DataContext context) : base(context)
        {

        }
    }
}
