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

    public interface IAgentActionsService
    {
        Task<bool> PatrolPoint(Point point, long bossId);
        Task ScheduleRefreshAgentsJob();
        Task MakeStepDuringPatrolling(long movingAgentId);
        Task MakeStepMovingWithLoot(long movingAgentId);
        Task SendToPatrol(PatrolRequest request);
        Task CancelAgentAmbush(long agentId, long bossId);
        Task<Point> GetAgentPosition(long agentId, long? bossId = null);
        Task<Agent> DismissAgent(DismissAgentRequest request);
        Task<Agent> RecruitAgent(RecruitAgentRequest request);
    }

    public class AgentActionsService : IAgentActionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAgentValidator _agentValidator;
        private readonly ISchedulerFactory _scheduler;
        private readonly IAgentFactory _agentFactory;
        private readonly IAgentRefreshJobRunner _agentRefreshJobRunner;
        private readonly ILogger<AgentActionsService> _logger;
        private readonly IPatrolJobRunner _patrolJobRunner;
        private readonly IReporter _reporter;
        private readonly IMovingAgentUtils _movingAgentUtils;
        private readonly IAgentUtils _agentUtils;

        public AgentActionsService(IUnitOfWork unitOfWork, IAgentValidator agentValidator, IAgentFactory agentFactory, ISchedulerFactory scheduler, 
            IAgentRefreshJobRunner agentRefreshJobRunner, ILogger<AgentActionsService> logger, IPatrolJobRunner patrolJobRunner, IReporter reporter,
            IMovingAgentUtils movingAgentUtils, IAgentUtils agentUtils)
        {
            _unitOfWork = unitOfWork;
            _agentValidator = agentValidator;
            _agentFactory = agentFactory;
            _scheduler = scheduler;
            _agentRefreshJobRunner = agentRefreshJobRunner;
            _logger = logger;
            _patrolJobRunner = patrolJobRunner;
            _reporter = reporter;
            _movingAgentUtils = movingAgentUtils;
            _agentUtils = agentUtils;
        }


        /// <summary>
        /// Boss dismisses an agent
        /// </summary>
        public async Task<Agent> DismissAgent(DismissAgentRequest request)
        {
            await _agentValidator.ValidateDismissAgent(request);
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            agent.StateIdEnum = AgentState.Renegate;
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
            agent.StateIdEnum = AgentState.Active;
            _unitOfWork.Commit();
            return agent;
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
                agent.StateIdEnum = AgentState.Active;
                _unitOfWork.MovingAgents.DeleteById(movingAgentId);
            }
            _unitOfWork.Commit();
        }

        public async Task MakeStepMovingWithLoot(long movingAgentId)
        {
            var movingAgent = await _unitOfWork.MovingAgents.GetByIdAsync(movingAgentId);
            if (movingAgent == null)
                throw new Exception($"Moving agent with id {movingAgentId} not found");
            var agent = movingAgent.Agent;
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
                    ambushAgent.StateIdEnum = AgentState.Active;
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
                    agent.StateIdEnum = AgentState.Active;
                    return;
                }
                //agent with loot wins
                else
                {
                    reportForAmbusher += "\nYour agent lost the shootout. He returns to his headquarters.";
                    _unitOfWork.Ambushes.DeleteById(ambush.Id);
                    _unitOfWork.MapElements.DeleteById(ambush.MapElementId);
                    ambushAgent.StateIdEnum = AgentState.Active;
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
                agent.StateIdEnum = AgentState.Active;
                agent.SubstateIdEnum = null;
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

        //NEW

        public async Task CancelAgentAmbush(long agentId, long bossId)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);
            var ambush = agent.Ambush;
            agent.StateIdEnum = AgentState.Active;
            _unitOfWork.MapElements.DeleteById(ambush.MapElementId);
            _unitOfWork.Commit();
        }

        public async Task<Point> GetAgentPosition(long agentId, long? bossId = null)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);

            if(bossId.HasValue && bossId.Value != agent.BossId)
            {
                throw new Exception("It is not your agent");
            }

            switch(agent.StateIdEnum)
            {
                case AgentState.OnMission:
                    return agent.PerformingMission.Mission.MapElement.Position;
                case AgentState.Moving:
                    return agent.MovingAgent.CurrentPoint;
                case AgentState.Ambushing:
                    return agent.Ambush.MapElement.Position;
                default:
                    throw new Exception($"Agent with state {Enum.GetName(typeof(AgentState), agent.StateIdEnum)} does not have any position");
            }
        }
    }
}
