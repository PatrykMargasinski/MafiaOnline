using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Factories;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
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
        Task<IList<AgentDTO>> GetBossAgents(long bossId);
        Task<IList<AgentDTO>> GetActiveAgents(long bossId);
        Task<IList<AgentOnMissionDTO>> GetAgentsOnMission(long bossId);
        Task<IList<AgentForSaleDTO>> GetAgentsForSale();
        Task<IList<MovingAgentDTO>> GetMovingAgents(long bossId);
        Task<IList<AmbushingAgentDTO>> GetAmbushingAgents(long bossId);
        Task<Agent> DismissAgent(DismissAgentRequest request);
        Task<Agent> RecruitAgent(RecruitAgentRequest request);
        Task<bool> PatrolPoint(Point point, long bossId);
        Task RefreshAgents();
        Task ScheduleRefreshAgentsJob();
        Task MakeStepDuringPatrolling(long movingAgentId);
        Task MakeStepMovingWithLoot(long movingAgentId);
        Task SendToPatrol(PatrolRequest request);
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
        private readonly IPatrolJobRunner _patrolJobRunner;
        private readonly IReturnWithLootJobRunner _returnWithLootJobRunner;
        private readonly IReporter _reporter;
        private readonly IMovingAgentUtils _movingAgentUtils;
        private readonly IAgentUtils _agentUtils;
        private readonly IMailSender _mailSender;

        public AgentService(IUnitOfWork unitOfWork, IMapper mapper, IAgentValidator agentValidator, IAgentFactory agentFactory, ISchedulerFactory scheduler, 
            IAgentRefreshJobRunner agentRefreshJobRunner, ILogger<AgentService> logger, IRandomizer randomizer, IPatrolJobRunner patrolJobRunner, IReporter reporter,
            IMovingAgentUtils movingAgentUtils, IAgentUtils agentUtils, IReturnWithLootJobRunner returnWithLootJobRunner, IMailSender mailSender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _agentValidator = agentValidator;
            _agentFactory = agentFactory;
            _scheduler = scheduler;
            _agentRefreshJobRunner = agentRefreshJobRunner;
            _logger = logger;
            _randomizer = randomizer;
            _patrolJobRunner = patrolJobRunner;
            _reporter = reporter;
            _movingAgentUtils = movingAgentUtils;
            _agentUtils = agentUtils;
            _returnWithLootJobRunner = returnWithLootJobRunner;
            _mailSender = mailSender;
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
            var boss = agent.Boss;
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

        public async Task SendToPatrol(PatrolRequest request)
        {
            await _agentValidator.ValidateSendToPatrol(request);
            var movingAgent = await _agentFactory.CreateMovingAgentForPatrolInstance(request);
            _unitOfWork.MovingAgents.Create(movingAgent);
            _unitOfWork.Commit();
            await _patrolJobRunner.Start(_scheduler, DateTime.Now.AddSeconds(MapConsts.SECONDS_TO_MAKE_ONE_STEP), movingAgent.Id);
        }

        public async Task<bool> PatrolPoint(Point point, long bossId)
        {
            var mapElement = await _unitOfWork.MapElements.GetInPoint(point.X, point.Y);
            if(mapElement != null && mapElement.Type == MapElementType.Ambush && mapElement.BossId != bossId)
            {
                await _movingAgentUtils.ExposeMapElement(mapElement.Id, bossId);
                await _reporter.CreateReport(bossId, "Ambush exposed", $"Ambush exposed at point [{mapElement.X},{mapElement.Y}]");
            }
            return false;
        }

        public async Task MakeStepDuringPatrolling(long movingAgentId)
        {
            var movingAgent = await _unitOfWork.MovingAgents.GetByIdAsync(movingAgentId);
            if (movingAgent == null)
                throw new Exception($"Moving agent with id {movingAgentId} not found");
            var agent = await _unitOfWork.Agents.GetByIdAsync(movingAgent.AgentId);
            await PatrolPoint(movingAgent.Path[movingAgent.Step.Value], agent.BossId.Value);
            movingAgent.Step = movingAgent.Step + 1;
            movingAgent.ArrivalTime = DateTime.Now.AddSeconds(movingAgent.StepsLeft.Value * MapConsts.SECONDS_TO_MAKE_ONE_STEP);
            if (movingAgent.Step >= movingAgent.Path.Length)
            {
                agent.State = AgentState.Active;
                _unitOfWork.MovingAgents.DeleteById(movingAgentId);
            }
            _unitOfWork.Commit();
        }

        public async Task MakeStepMovingWithLoot(long movingAgentId)
        {
            var movingAgent = await _unitOfWork.MovingAgents.GetByIdAsync(movingAgentId);
            if (movingAgent == null)
                throw new Exception($"Moving agent with id {movingAgentId} not found");
            var agent = await _unitOfWork.Agents.GetByIdAsync(movingAgent.AgentId);
            var point = movingAgent.Path[movingAgent.Step.Value];
            var mapElement = await _unitOfWork.MapElements.GetInPoint(point.X, point.Y);
            if (mapElement != null && mapElement.Type == MapElementType.Ambush && mapElement.BossId != agent.BossId)
            {
                var reportForAmbusher = "Your agent's ambush worked. Some other agent busted in with it and there's a chance to take his loot.\n";
                var reportForAgentWithLoot = "Your agent was ambushed. There is a chance that he will lose his loot.\n";
                var ambush = await _unitOfWork.Ambushes.GetByMapElementIdAsync(mapElement.Id);
                var ambushAgent = await _unitOfWork.Agents.GetByIdAsync(ambush.AgentId);
                var shootoutResult = await _agentUtils.Shootout(agent.Id, ambush.AgentId, ambush.AgentId);
                reportForAmbusher += shootoutResult.Report;
                reportForAgentWithLoot += shootoutResult.Report;

                //ambusher wins
                if (shootoutResult.WinnerAgentId == ambushAgent.Id)
                {
                    reportForAmbusher += "\nYour agent won the shootout. You get loot.";
                    reportForAgentWithLoot += "\nYour agent lost the shootout. His loot was taken by the enemy.";
                    _unitOfWork.Ambushes.DeleteById(ambush.Id);
                    _unitOfWork.MapElements.DeleteById(ambush.MapElementId);
                    ambushAgent.State = AgentState.Active;
                    var ambushAgentBoss = await _unitOfWork.Bosses.GetByIdAsync(ambushAgent.BossId.Value);
                    var loot = JsonSerializer.Deserialize<Loot>(movingAgent.DatasJson);
                    ambushAgentBoss.Money += loot.Money;
                    var scheduler = await _scheduler.GetScheduler();
                    JobKey jobKey = new(movingAgent.JobKey, "group1");
                    if (await scheduler.CheckExists(jobKey))
                    {
                        await scheduler.DeleteJob(jobKey);
                    }
                    _unitOfWork.MovingAgents.DeleteById(movingAgentId);
                    agent.State = AgentState.Active;
                    return;
                }
                //agent with loot wins
                else
                {
                    reportForAmbusher += "\nYour agent lost the shootout. He returns to his headquarters.";
                    _unitOfWork.Ambushes.DeleteById(ambush.Id);
                    _unitOfWork.MapElements.DeleteById(ambush.MapElementId);
                    ambushAgent.State = AgentState.Active;
                    reportForAgentWithLoot += "\nYour agent won the shootout. His loot is still safe.";
                }

                await _reporter.CreateReport(ambushAgent.BossId.Value, "Shootout", reportForAmbusher);
                await _reporter.CreateReport(agent.BossId.Value, "Shootout", reportForAgentWithLoot);
            }
            movingAgent.Step = movingAgent.Step + 1;
            movingAgent.ArrivalTime = DateTime.Now.AddSeconds(movingAgent.StepsLeft.Value * MapConsts.SECONDS_TO_MAKE_ONE_STEP);

            if (movingAgent.Step >= movingAgent.Path.Length)
            {
                var loot = JsonSerializer.Deserialize<Loot>(movingAgent.DatasJson);
                var boss = await _unitOfWork.Bosses.GetByIdAsync(agent.BossId.Value);
                boss.Money += loot.Money;
                agent.State = AgentState.Active;
                var report = "Your agent returned to the headquarter with loot. The money is yours and no one undesirable will lay hands on it";
                await _reporter.CreateReport(boss.Id, "Agent returned to HQ with loot", report);
                _unitOfWork.MovingAgents.DeleteById(movingAgentId);
            }
            _unitOfWork.Commit();
        }

        public async Task ScheduleRefreshAgentsJob()
        {
            await _agentRefreshJobRunner.Start(_scheduler, DateTime.Now.AddMinutes(AgentConsts.MINUTES_TO_REFRESH_AGENTS_FOR_SALE));
        }
    }
}
