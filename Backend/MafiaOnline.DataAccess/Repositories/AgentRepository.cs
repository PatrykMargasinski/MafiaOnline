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
        Task<IList<Agent>> GetAgentsForSale();
        Task<IList<Agent>> GetRenegates();
    }

    public class AgentRepository : CrudRepository<Agent>, IAgentRepository
    {
        public AgentRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<Agent>> GetActiveAgents(long bossId)
        {
            var agents = await _context.Agents
                .Where(x => x.State == AgentState.Active && x.BossId==bossId)
                .ToListAsync();
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
                .Where(z => z.BossId == bossId && z.State == AgentState.OnMission)
                .ToListAsync();
            return agents;
        }

        public async Task<IList<Agent>> GetAgentsForSale()
        {
            var agents = await _context.Agents
                .Include(x => x.AgentForSale)
                .Where(z => z.State == AgentState.ForSale)
                .ToListAsync();
            return agents;
        }

        public async Task<IList<Agent>> GetRenegates()
        {
            var agents = await _context.Agents
                .Where(z => z.State == AgentState.Renegate)
                .ToListAsync();
            return agents;
        }

        public async override Task<Agent> GetByIdAsync(long agentId)
        {
            var agent = await _context.Agents
                .Include(x => x.AgentForSale)
                .FirstOrDefaultAsync(y=>y.Id == agentId);
            return agent;
        }

        public async override Task<IList<Agent>> GetByIdsAsync(long[] agentsId)
        {
            var agent = await _context.Agents
                .Include(x => x.AgentForSale)
                .ToListAsync();
            return agent;
        }
    }
}
