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
    public interface IAgentRepository : ICrudRepository<Agent>
    {
        Task<IList<Agent>> GetActiveAgents(long bossId);
        Task<IList<Agent>> GetBossAgents(long bossId);
        Task<IList<Agent>> GetAgentsOnMission(long bossId);
    }

    public class AgentRepository : CrudRepository<Agent>, IAgentRepository
    {
        public AgentRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<Agent>> GetActiveAgents(long bossId)
        {
            var agents = await _context.Agents.Where(x => x.State == AgentState.Active && x.BossId==bossId).ToListAsync();
            return agents;
        }

        public async Task<IList<Agent>> GetBossAgents(long bossId)
        {
            var agents = await _context.Agents
                .Where(x => x.BossId == bossId)
                .ToListAsync();
            return agents;
        }

        public async Task<IList<Agent>> GetAgentsOnMission(long bossId)
        {
            var agents = await _context.Agents
                .Include(x => x.PerformingMission)
                .ThenInclude(y => y.Mission)
                .Where(z => z.BossId == bossId)
                .ToListAsync();
            return agents;
        }
    }
}
