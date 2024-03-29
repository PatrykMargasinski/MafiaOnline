﻿using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Factories;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IMissionService
    {
        Task StartMission(StartMissionRequest request);
        Task<PerformingMission> CreatePerformingMissionInstance(long agentId, long missionId, Point[] path);
        Task EndMission(long pmId);
        Task<MissionDTO> GetMissionByMapElement(long mapElementId);
        Task<IList<MissionDTO>> GetAvailableMissions();
        Task<IList<PerformingMissionDTO>> GetPerformingMissions(long bossId);
        Task RefreshMissions();
        Task ScheduleRefreshMissionsJob();
        Task<MissionDTO> GetMissionById(long missionId);
        Task MoveOnMission(StartMissionRequest request);
    }

    public class MissionService : IMissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISchedulerFactory _scheduler;
        private readonly IMissionUtils _missionUtils;
        private readonly IReporter _reporter;
        private readonly IMissionValidator _missionValidator;
        private readonly ILogger<MissionService> _logger;
        private readonly IPerformMissionJobRunner _jobRunner;
        private readonly IMissionFactory _missionFactory;
        private readonly IAgentFactory _agentFactory;
        private readonly IMissionRefreshJobRunner _missionRefreshJobRunner;
        private readonly IAgentMovingOnMissionJobRunner _agentMoveJobRunner;
        private readonly IReturnWithLootJobRunner _returnWithLootJobRunner;

        public MissionService(IUnitOfWork unitOfWork, IMapper mapper, ISchedulerFactory scheluder, IMissionUtils missionUtils, IReporter reporter, IMissionValidator missionValidator,
            IPerformMissionJobRunner jobRunner, ILogger<MissionService> logger, IMissionFactory missionFactory, IMissionRefreshJobRunner missionRefreshJobRunner, IAgentMovingOnMissionJobRunner agentMoveJobRunner,
            IReturnWithLootJobRunner returnWithLootJobRunner, IAgentFactory agentFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduler = scheluder;
            _missionUtils = missionUtils;
            _reporter = reporter;
            _missionValidator = missionValidator;
            _jobRunner = jobRunner;
            _logger = logger;
            _missionFactory = missionFactory;
            _missionRefreshJobRunner = missionRefreshJobRunner;
            _agentMoveJobRunner = agentMoveJobRunner;
            _returnWithLootJobRunner = returnWithLootJobRunner;
            _agentFactory = agentFactory;
        }

        /// <summary>
        /// Agents moves on mission. It will be performed when agent reaches a place
        /// </summary>
        public async Task MoveOnMission(StartMissionRequest request)
        {
            await _missionValidator.ValidateMoveOnMission(request);
            var movingAgent = await _missionFactory.CreateAgentMovingOnMission(request);
            _unitOfWork.MovingAgents.Create(movingAgent);
            _unitOfWork.Commit();
            await _agentMoveJobRunner.Start(_scheduler, movingAgent.ArrivalTime, movingAgent.Id);
        }

        /// <summary>
        /// Starts mission. It will be performed after a time depending on the mission
        /// </summary>
        public async Task StartMission(StartMissionRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var mission = await _unitOfWork.Missions.GetByIdAsync(request.MissionId);
            try
            {
                var pm = await CreatePerformingMissionInstance(request.AgentId, request.MissionId, request.Path);
                await _jobRunner.Start(_scheduler, pm.Id, pm.CompletionTime);
            }
            catch(Exception ex)
            {
                string messageContent;
                messageContent = $"Agent {agent.FullName ?? ""} wanted to perform ";
                if (mission != null) messageContent += $"mission {mission.Name}";
                else messageContent += $"some mission";
                messageContent += ", but there are reasons why he couldn't:\n";
                messageContent += $"{ex.Message}\n\n";
                messageContent += "The agent returns to the headquarters";
                await _reporter.CreateReport(agent.BossId.Value, "The agent returns to the headquarters", messageContent);
                agent.StateIdEnum = AgentState.Active;
                _unitOfWork.Commit();
                return;
            }
        }

        /// <summary>
        /// Creates PerformingMission instance
        /// </summary>
        public async Task<PerformingMission> CreatePerformingMissionInstance(long agentId, long missionId, Point[] path)
        {

            await _missionValidator.ValidateDoMission(agentId, missionId);

            var mission = await _unitOfWork.Missions.GetByIdAsync(missionId);
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);

            await _reporter.CreateReport(agent.BossId.Value, 
                $"Agent {agent.FullName} starts mission {mission.Name}", 
                $"Agent {agent.FullName} starts mission {mission.Name}. He/she will finish it in {mission.Duration} seconds and after it he/she will go back to the headquarters");

            DateTime missionFinishTime = DateTime.Now.AddSeconds(mission.Duration);

            PerformingMission performingMission = new PerformingMission()
            {
                MissionId = missionId,
                AgentId = agentId,
                CompletionTime = missionFinishTime,
                WayBack = path
            };
            agent.StateIdEnum = AgentState.OnMission;
            mission.StateIdEnum = MissionState.Performing;
            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(mission.MapElementId);
            mapElement.Boss = agent.Boss;
            _unitOfWork.PerformingMissions.Create(performingMission);
            _unitOfWork.Commit();
            return performingMission;
        }

        /// <summary>
        /// Ends mission and sends report to the boss informing about the result of the mission
        /// </summary>
        public async Task EndMission(long pmId)
        {
            await _missionValidator.ValidateEndMission(pmId);

            PerformingMission pm = await _unitOfWork.PerformingMissions.GetByIdAsync(pmId);
            Agent agent = pm.Agent;
            Mission mission = pm.Mission;
            Boss boss = agent.Boss;
            string info = "Agent " + agent.FirstName + " " + agent.LastName +
                " has finished mission: " + mission.Name;

            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(mission.MapElementId);
            if (_missionUtils.IsMissionSuccessfullyCompleted(agent, mission))
            {
                info += "\nMission success!\n";
                info += boss.LastName +
                "family has chance to earn " + mission.Loot +
                "\nNow the agent returns to the headquarters with the loot. Let's hope he doesn't fall into any ambush";
                var movingAgent = await _agentFactory.CreateMovingAgentWithLoot(agent.Id, mission.Loot, pm.WayBack);
                _unitOfWork.MovingAgents.Create(movingAgent);
                _unitOfWork.Commit();
                await _returnWithLootJobRunner.Start(_scheduler, DateTime.Now.AddSeconds(MapConsts.SECONDS_TO_MAKE_ONE_STEP), movingAgent.Id);

                if (mission.RepeatableMission == true)
                {
                    mission.StateIdEnum = MissionState.Available;
                    mapElement.Boss = null;
                    mapElement.BossId = null;
                }
                else
                {
                    _unitOfWork.Missions.DeleteById(mission.Id);
                    _unitOfWork.MapElements.DeleteById(mission.MapElementId);
                }
            }
            else
            {
                info += ("\nMission failed!\nThe agent returns to the headquarters\n");
                agent.StateIdEnum = AgentState.Active;
            }

            _unitOfWork.PerformingMissions.DeleteById(pm.Id);
            await _reporter.CreateReport(boss.Id, "Mission completed: " + mission.Name, info);

            _unitOfWork.Commit();
        }

        /// <summary>
        /// Returns available missions
        /// </summary>
        public async Task<IList<MissionDTO>> GetAvailableMissions()
        {
            var missions = await _unitOfWork.Missions.GetAvailableMissions();
            return _mapper.Map<IList<MissionDTO>>(missions);
        }

        public async Task<MissionDTO> GetMissionById(long missionId)
        {
            var mission = await _unitOfWork.Missions.GetByIdAsync(missionId);
            return _mapper.Map<MissionDTO>(mission);
        }

        /// <summary>
        /// Returns mission details by map element
        /// </summary>
        public async Task<MissionDTO> GetMissionByMapElement(long mapElementId)
        {
            var missions = await _unitOfWork.Missions.GetByMapElementIdAsync(mapElementId);
            return _mapper.Map<MissionDTO>(missions);
        }

        /// <summary>
        /// Returns missions performing by boss with id=bossId
        /// </summary>
        public async Task<IList<PerformingMissionDTO>> GetPerformingMissions(long bossId)
        {
            var missions = await _unitOfWork.Missions.GetPerformingMissions(bossId);
            return _mapper.Map<IList<PerformingMissionDTO>>(missions);
        }

        public async Task RefreshMissions()
        {
            _logger.LogInformation("Refreshing missions started at: " + DateTime.Now.ToString());
            var missions = await _unitOfWork.Missions.GetAllAsync();
            var countOfNewMissions = MissionConsts.MAX_NUMBER_OF_MISSIONS - missions.Count;
            var templates = await _unitOfWork.MissionTemplates.GetRandomsAsync(countOfNewMissions);
            foreach(var template in templates)
            {
                var newMission = await _missionFactory.CreateByMissionTemplate(template);
                _unitOfWork.Missions.Create(newMission);
                _logger.LogInformation("Mission from template" + template.Id + " added" );
            }
            _unitOfWork.Commit();
        }

        public async Task ScheduleRefreshMissionsJob()
        {
            await _missionRefreshJobRunner.Start(_scheduler, DateTime.Now.AddMinutes(MissionConsts.MINUTES_TO_REFRESH_MISSIONS));
        }
    }
}
