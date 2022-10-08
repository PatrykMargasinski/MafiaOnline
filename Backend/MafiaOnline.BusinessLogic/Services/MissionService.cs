using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
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
        Task StartMission(long agentId, long missionId);
        Task<PerformingMission> DoMission(long agentId, long missionId);
        Task EndMission(long pmId);
        Task<IList<MissionDTO>> GetAvailableMissions();
        Task<IList<PerformingMissionDTO>> GetPerformingMissions(long bossId);
    }

    public class MissionService : IMissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISchedulerFactory _scheduler;
        private readonly IMissionUtils _missionUtils;
        private readonly IReporter _reporter;
        private readonly IMissionValidator _missionValidator;

        public MissionService(IUnitOfWork unitOfWork, IMapper mapper, ISchedulerFactory scheluder, IMissionUtils missionUtils, IReporter reporter, IMissionValidator missionValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduler = scheluder;
            _missionUtils = missionUtils;
            _reporter = reporter;
            _missionValidator = missionValidator;
        }

        /// <summary>
        /// Starts mission. It will be performed after a time depending on the mission
        /// </summary>
        public async Task StartMission(long agentId, long missionId)
        {
            var pm = await DoMission(agentId, missionId);
            await MissionJob.Start(_scheduler, pm.Id, pm.CompletionTime);
        }

        /// <summary>
        /// Creates PerformingMission instance
        /// </summary>
        public async Task<PerformingMission> DoMission(long agentId, long missionId)
        {

            await _missionValidator.ValidateDoMission(agentId, missionId);

            var mission = await _unitOfWork.Missions.GetByIdAsync(missionId);
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);
            DateTime missionFinishTime = DateTime.Now.AddSeconds(mission.Duration);

            PerformingMission performingMission = new PerformingMission()
            {
                MissionId = missionId,
                AgentId = agentId,
                CompletionTime = missionFinishTime
            };
            agent.State = AgentState.OnMission;
            mission.State = MissionState.Performing;
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
            Agent agent = await _unitOfWork.Agents.GetByIdAsync(pm.AgentId);
            Mission mission = await _unitOfWork.Missions.GetByIdAsync(pm.MissionId);

            long bossId = agent.BossId.Value;
            Boss boss = await _unitOfWork.Bosses.GetByIdAsync(bossId);
            string info = "Agent " + agent.FirstName + " " + agent.LastName +
                " has finished mission: " + mission.Name;
            if (_missionUtils.IsMissionSuccessfullyCompleted(agent, mission))
            {
                boss.Money+=mission.Loot;
                info += "\nMission success! \n";
                info += boss.LastName +
                " family has earned " + mission.Loot + "$";
            }
            else
            {
                info += ("\nMission failed.\n");
            }
            agent.State = AgentState.Active;
            mission.State = MissionState.Available;
            await _reporter.CreateReport(bossId, "Mission: " + mission.Name, info);
            _unitOfWork.PerformingMissions.DeleteById(pm.Id);
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

        /// <summary>
        /// Returns missions performing by boss with id=bossId
        /// </summary>
        public async Task<IList<PerformingMissionDTO>> GetPerformingMissions(long bossId)
        {
            var missions = await _unitOfWork.Missions.GetPerformingMissions(bossId);
            return _mapper.Map<IList<PerformingMissionDTO>>(missions);
        }
    }
}
