﻿using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using MafiaOnline.DataAccess.Entities.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        Task<IList<Agent>> GetMovingAgents(long bossId);
        Task<IList<Agent>> GetRenegates();
        Task<IList<Agent>> GetAmbushingAgents(long bossId);
        Task<IList<VAgent>> GetAgentByQuery(AgentQuery query);
    }

    public class AgentRepository : CrudRepository<Agent>, IAgentRepository
    {
        public AgentRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<Agent>> GetActiveAgents(long bossId)
        {
            var agents = await _context.Agents
                .Where(x => x.StateId == (long?)AgentState.Active && x.BossId==bossId)
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
                .ThenInclude(y => y.MapElement)
                .Where(z => z.BossId == bossId && z.StateId == (long?)AgentState.OnMission)
                .ToListAsync();
            return agents;
        }

        public async Task<IList<Agent>> GetAgentsForSale()
        {
            var agents = await _context.Agents
                .Include(x => x.AgentForSale)
                .Where(z => z.StateId == (long?)AgentState.ForSale)
                .ToListAsync();
            return agents;
        }

        public async Task<IList<Agent>> GetMovingAgents(long bossId)
        {
            var agents = await _context.Agents
                .Include(x => x.MovingAgent)
                .Where(z => (z.StateId == (long?)AgentState.Moving) && z.BossId==bossId)
                .ToListAsync();
            return agents;
        }


        public async Task<IList<Agent>> GetAmbushingAgents(long bossId)
        {
            var agents = await _context.Agents
                .Include(x => x.Ambush)
                .ThenInclude(x => x.MapElement)
                .Where(z => z.StateId == (long?)AgentState.Ambushing && z.BossId == bossId)
                .ToListAsync();
            return agents;
        }

        public async Task<IList<Agent>> GetRenegates()
        {
            var agents = await _context.Agents
                .Where(z => z.StateId == (long?)AgentState.Renegate)
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
                .Where(x=>agentsId.Contains(x.Id))
                .ToListAsync();
            return agent;
        }

        public override void DeleteById(long id)
        {
            base.DeleteById(id);
            var agentForSale = _context.AgentsForSale.Where(x=> x.AgentId == id).FirstOrDefault();
            if (agentForSale != null)
                _context.AgentsForSale.Remove(agentForSale);
        }

        public override void DeleteByIds(long[] ids)
        {
            base.DeleteByIds(ids);
            var agentsForSale = _context.AgentsForSale.Where(x => ids.Contains(x.AgentId)).ToList();
            if (agentsForSale.Count!=0)
                _context.AgentsForSale.RemoveRange(agentsForSale);
        }

        public async Task<IList<VAgent>> GetAgentByQuery(AgentQuery query)
        {
            IQueryable<VAgent> queryable = _context.VAgent;

            queryable = queryable.Where(x => x.BossId == query.BossId);

            if(!string.IsNullOrEmpty(query.Name))
            {
                queryable = queryable.Where(x => (x.FirstName + " " + x.LastName).Contains(query.Name));
            }

            if(query.State.HasValue)
            {
                queryable = queryable.Where(x => x.StateId == (long?)query.State);
            }

            queryable = query.ApplySorting(queryable);
            queryable = query.ApplyPaging(queryable);

            return await queryable.ToListAsync();
        }
    }
}
