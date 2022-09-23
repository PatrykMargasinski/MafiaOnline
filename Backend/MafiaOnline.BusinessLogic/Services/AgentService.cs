using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IAgentService
    {
        Task<IList<AgentDTO>> GetAllAgents();
        Task<IList<AgentDTO>> GetBossAgents(long bossId);
        Task<IList<AgentDTO>> GetActiveAgents(long bossId);
        Task<IList<AgentOnMissionDTO>> GetAgentsOnMission(long bossId);
        Task<IList<AgentForSaleDTO>> GetAgentsForSale();
        Task<Agent> AbandonAgent(long id);
        Task<Agent> RecruitAgent(long bossId, long agentId);

    }

    public class AgentService : IAgentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AgentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all agents in the database
        /// </summary>
        public async Task<IList<AgentDTO>> GetAllAgents()
        {
            var agents = await _unitOfWork.Agents.GetAllAsync();
            return _mapper.Map<IList<AgentDTO>>(agents);
        }

        /// <summary>
        /// Returns agents belonging to the boss
        /// </summary>
        public async Task<IList<AgentDTO>> GetBossAgents(long bossId)
        {
            var agents = await _unitOfWork.Agents.GetBossAgents(bossId);
            return _mapper.Map<IList<AgentDTO>>(agents);
        }

        /// <summary>
        /// Returns active agents belonging to the boss
        /// </summary>
        public async Task<IList<AgentDTO>> GetActiveAgents(long bossId)
        {
            var agents = await _unitOfWork.Agents.GetActiveAgents(bossId);
            return _mapper.Map<IList<AgentDTO>>(agents);
        }

        /// <summary>
        /// Returns agents on mission belonging to the boss
        /// </summary>
        public async Task<IList<AgentOnMissionDTO>> GetAgentsOnMission(long bossId)
        {
            var agents = await _unitOfWork.Agents.GetAgentsOnMission(bossId);
            return _mapper.Map<IList<AgentOnMissionDTO>>(agents);
        }

        /// <summary>
        /// Returns agents for sale
        /// </summary>
        public async Task<IList<AgentForSaleDTO>> GetAgentsForSale()
        {
            var agents = await _unitOfWork.Agents.GetAgentsForSale();
            return _mapper.Map<IList<AgentForSaleDTO>>(agents);
        }

        /// <summary>
        /// Agent abandons his boss
        /// </summary>
        public async Task<Agent> AbandonAgent(long agentId)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);
            if (agent == null)
                throw new Exception("Agent not found");
            if (agent.State != AgentState.Active)
                throw new Exception("Agent isn't active - he cannot be abandoned");
            if (agent.BossId == null)
                throw new Exception("Agent doesn't belong to any boss");
            agent.State = AgentState.Renegate;
            agent.Boss = null;
            agent.BossId = null;
            _unitOfWork.Commit();
            return agent;
        }

        /// <summary>
        /// Boss recruits an agent
        /// </summary>
        public async Task<Agent> RecruitAgent(long bossId, long agentId)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);
            if (agent == null)
                throw new Exception("Agent not found");
            var boss = await _unitOfWork.Bosses.GetByIdAsync(bossId);
            if (agent.State != AgentState.OnSale)
                throw new Exception("Agent is not for sale");
            if (agent.AgentForSale == null)
                throw new Exception("There is no AgentForSale instance");
            if (boss == null)
                throw new Exception("Boss not found");
            if (agent.AgentForSale.Price > boss.Money)
                throw new Exception("Boss cannot afford the agent");
            boss.Money -= agent.AgentForSale.Price;
            _unitOfWork.AgentsForSale.DeleteByAgentId(agentId);
            agent.Boss = boss;
            agent.State = AgentState.Active;
            _unitOfWork.Commit();
            return agent;
        }
    }
}
