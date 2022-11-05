using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Factories;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
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
        Task<Agent> DismissAgent(DismissAgentRequest request);
        Task<Agent> RecruitAgent(RecruitAgentRequest request);
        Task RefreshAgents();
        Task ScheduleRefreshAgentsJob();

    }

    public class AgentService : IAgentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAgentValidator _agentValidator;
        private readonly ISchedulerFactory _scheduler;
        private readonly IAgentFactory _agentFactory;
        private readonly IAgentRefreshJobRunner _agentRefreshJobRunner;
        private readonly ILogger<AgentService> _logger;
        private readonly IRandomizer _randomizer;

        public AgentService(IUnitOfWork unitOfWork, IMapper mapper, IAgentValidator agentValidator, IAgentFactory agentFactory, ISchedulerFactory scheduler, IAgentRefreshJobRunner agentRefreshJobRunner, ILogger<AgentService> logger, IRandomizer randomizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _agentValidator = agentValidator;
            _agentFactory = agentFactory;
            _scheduler = scheduler;
            _agentRefreshJobRunner = agentRefreshJobRunner;
            _logger = logger;
            _randomizer = randomizer;
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
        /// Boss dismisses an agent
        /// </summary>
        public async Task<Agent> DismissAgent(DismissAgentRequest request)
        {
            await _agentValidator.ValidateDismissAgent(request);
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            agent.State = AgentState.Renegate;
            agent.Boss = null;
            agent.BossId = null;
            _unitOfWork.Commit();
            return agent;
        }

        /// <summary>
        /// Boss recruits an agent
        /// </summary>
        public async Task<Agent> RecruitAgent(RecruitAgentRequest request)
        {
            await _agentValidator.ValidateRecruitAgent(request);
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(request.BossId);
            boss.Money -= agent.AgentForSale.Price;
            _unitOfWork.AgentsForSale.DeleteByAgentId(request.AgentId);
            agent.Boss = boss;
            agent.State = AgentState.Active;
            _unitOfWork.Commit();
            return agent;
        }

        /// <summary>
        /// Replenishes agents for sale
        /// </summary>
        public async Task RefreshAgents()
        {
            _logger.LogInformation("Refreshing agents started at: " + DateTime.Now.ToString());
            var agentsForSale = await _unitOfWork.Agents.GetAgentsForSale();
            var renegates = await _unitOfWork.Agents.GetRenegates();

            //removing agents being too long for sale
            var agentsToRemove = agentsForSale.Where(x => DateTime.Now > x.AgentForSale.StartOfSale.AddMinutes(AgentConsts.MINUTES_TO_REMOVE_FROM_SALE));
            IList<long> removedIds = new List<long>();

            foreach(var agent in agentsToRemove)
            {
                if(_randomizer.Next(2)%2==1)
                {
                    removedIds.Add(agent.Id);
                    
                }
            }

            _logger.LogInformation("Agents to remove: " + string.Join(',',removedIds));

            _unitOfWork.Agents.DeleteByIds(removedIds.ToArray());

            agentsForSale = agentsForSale.Where(x => !removedIds.Contains(x.Id)).ToList();

            //replenishment of agents for sale
            for (int i = agentsForSale.Count; i < AgentConsts.NUMBER_OF_AGENTS_FOR_SALE; i++)
            {
                Agent newAgent;

                //50% chance that renegate agent become for sale, 50% that there will be new agent created
                if (_randomizer.Next(2)%2==1 && renegates.Count!=0)
                {
                    newAgent = renegates[_randomizer.Next(0, renegates.Count)];
                    renegates.Remove(newAgent);
                }
                else
                {
                    newAgent = await _agentFactory.Create();
                    _unitOfWork.Agents.Create(newAgent);

                }
                var agentForSale = await _agentFactory.CreateForSaleInstance(newAgent);
                _unitOfWork.AgentsForSale.Create(agentForSale);
            }
            _unitOfWork.Commit();
        }

        public async Task ScheduleRefreshAgentsJob()
        {
            await _agentRefreshJobRunner.Start(_scheduler, DateTime.Now.AddMinutes(AgentConsts.MINUTES_TO_REFRESH_AGENTS_FOR_SALE));
        }
    }
}
