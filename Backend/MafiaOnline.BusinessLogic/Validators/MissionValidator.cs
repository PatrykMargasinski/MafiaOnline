using AutoMapper;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Validators
{
    public interface IMissionValidator
    {
        Task ValidateDoMission(long agentId, long missionId);
        Task ValidateEndMission(long pmId);
    }

    public class MissionValidator : IMissionValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MissionValidator(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ValidateDoMission(long agentId, long missionId)
        {
            Mission mission = await _unitOfWork.Missions.GetByIdAsync(missionId);
            if (mission == null)
            {
                throw new Exception("Mission " + missionId + " not found!");
            }

            if (mission.State != MissionState.Available)
            {
                throw new Exception("Mission " + missionId + " is not available");
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

            if (agent.State != AgentState.Active)
            {
                throw new Exception("Agent " + agentId + " is not active");
            }
        }

        public async Task ValidateEndMission(long pmId)
        {
            PerformingMission pm = await _unitOfWork.PerformingMissions.GetByIdAsync(pmId);
            if (pm == null)
            {
                throw new Exception($"Performing mission with id {pmId} not found");
            }

            Agent agent = await _unitOfWork.Agents.GetByIdAsync(pm.AgentId);
            if(agent == null)
            {
                throw new Exception($"Agent with id {pm.AgentId} not found");
            }

            if (agent.BossId == null)
            {
                throw new Exception($"Agent with id {agent.Id} has no boss but he went on mission!");
            }

            Mission mission = await _unitOfWork.Missions.GetByIdAsync(pm.MissionId);
            if (mission == null)
            {
                throw new Exception($"Mission with id {mission.Id} not found!");
            }

        }
    }
}
