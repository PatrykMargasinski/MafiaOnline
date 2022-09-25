using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
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
    }

    public class MissionService : IMissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISchedulerFactory _scheduler;
        private readonly IMissionUtils _missionUtils;

        public MissionService(IUnitOfWork unitOfWork, IMapper mapper, ISchedulerFactory scheluder, IMissionUtils missionUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduler = scheluder;
            _missionUtils = missionUtils;
        }

        public async Task StartMission(long agentId, long missionId)
        {
            var pm = await DoMission(agentId, missionId);
            await MissionJob.Start(_scheduler, pm.Id, pm.CompletionTime);
        }

        public async Task<PerformingMission> DoMission(long agentId, long missionId)
        {
            Mission mission = await _unitOfWork.Missions.GetByIdAsync(missionId);
            if (mission == null)
            {
                throw new Exception("Mission with id " + missionId + " not found!");
            }
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);

            if (agent == null)
            {
                throw new Exception("Agent with id " + agentId + " not found!");
            }

            if (agent.BossId == null)
            {
                throw new Exception("Agent " + agentId + " is without boss");
            }
            DateTime missionFinishTime = DateTime.Now.AddSeconds(mission.Duration);

            PerformingMission performingMission = new PerformingMission()
            {
                MissionId = missionId,
                AgentId = agentId,
                CompletionTime = missionFinishTime
            };
            agent.State = AgentState.OnMission;
            _unitOfWork.PerformingMissions.Create(performingMission);
            _unitOfWork.Commit();
            return performingMission;
        }

        public async Task EndMission(long pmId)
        {
            PerformingMission pm = await _unitOfWork.PerformingMissions.GetByIdAsync(pmId);
            if (pm == null)
            {
                throw new Exception("No pm with given id " + pmId + "!");
            }
            Agent agent = await _unitOfWork.Agents.GetByIdAsync(pm.AgentId);
            Mission mission = await _unitOfWork.Missions.GetByIdAsync(pm.MissionId);
            if (agent.BossId==null)
            {
                throw new Exception("Agent with id " + agent.Id + " has no boss but he went on mission!");
            }
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
            //_reportRepository.CreateReport(bossId, "Mission: " + mission.Name, info);
            _unitOfWork.PerformingMissions.DeleteById(pm.Id);
            _unitOfWork.Commit();
        }
    }
}
