using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Factories;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using MafiaOnline.DataAccess.Entities.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IAgentService
    {
        Task<IList<AgentDTO>> GetAllAgents();
        Task<IList<AgentDTO>> GetAgentsByQuery(AgentQuery query);
        Task<IList<AgentDTO>> GetBossAgents(long bossId);
        Task<IList<AgentDTO>> GetActiveAgents(long bossId);
        Task<IList<AgentOnMissionDTO>> GetAgentsOnMission(long bossId);
        Task<IList<AgentForSaleDTO>> GetAgentsForSale();
        Task<IList<MovingAgentDTO>> GetMovingAgents(long bossId);
        Task<IList<AmbushingAgentDTO>> GetAmbushingAgents(long bossId);
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

        public AgentService(IUnitOfWork unitOfWork, IMapper mapper, IAgentValidator agentValidator, IAgentFactory agentFactory, ILogger<AgentService> logger, 
            IRandomizer randomizer, ISchedulerFactory scheduler, IAgentRefreshJobRunner agentRefreshJobRunner)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _agentValidator = agentValidator;
            _agentFactory = agentFactory;
            _logger = logger;
            _randomizer = randomizer;
            _scheduler = scheduler;
            _agentRefreshJobRunner = agentRefreshJobRunner;
        }

        /// <summary>
        /// Returns agents belonging to the boss
        /// </summary>
        public async Task<IList<AgentDTO>> GetAgentsByQuery(AgentQuery query)
        {
            var agents = await _unitOfWork.Agents.GetAgentByQuery(query);
            return _mapper.Map<IList<AgentDTO>>(agents);
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
        /// Returns moving agents belonging to the boss
        /// </summary>
        public async Task<IList<MovingAgentDTO>> GetMovingAgents(long bossId)
        {
            var agents = await _unitOfWork.Agents.GetMovingAgents(bossId);
            return _mapper.Map<IList<MovingAgentDTO>>(agents);
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
        /// Returns ambushing agents belonging to the boss
        /// </summary>
        public async Task<IList<AmbushingAgentDTO>> GetAmbushingAgents(long bossId)
        {
            var agents = await _unitOfWork.Agents.GetAmbushingAgents(bossId);
            return _mapper.Map<IList<AmbushingAgentDTO>>(agents);
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
