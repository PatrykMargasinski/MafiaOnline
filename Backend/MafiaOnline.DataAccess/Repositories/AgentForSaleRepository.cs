using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface IAgentForSaleRepository : ICrudRepository<AgentForSale>
    {
        void DeleteByAgentId(long agentId);
    }

    public class AgentForSaleRepository : CrudRepository<AgentForSale>, IAgentForSaleRepository
    {
        public AgentForSaleRepository(DataContext context) : base(context)
        {

        }

        public virtual void DeleteByAgentId(long agentId)
        {
            entities.Remove(entities.FirstOrDefault(x=>x.AgentId==agentId));
        }
    }
}
