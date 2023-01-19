using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
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
        Task ValidateMoveOnMission(StartMissionRequest request);
    }

    public class MissionValidator : IMissionValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapUtils _mapUtils;

        public MissionValidator(IUnitOfWork unitOfWork, IMapper mapper, IMapUtils mapUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapUtils = mapUtils;
        }

        public async Task ValidateMoveOnMission(StartMissionRequest request)
        {
            Mission mission = await _unitOfWork.Missions.GetByIdAsync(request.MissionId);
            if (mission == null)
            {
                throw new Exception("Mission " + request.MissionId + " not found!");
            }

            if (mission.State != MissionState.Available)
            {
                throw new Exception("Mission " + request.MissionId + " is not available");
            }

            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);

            if (agent == null)
            {
                throw new Exception("Agent with id " + request.AgentId + " not found!");
            }

            if (agent.BossId != request.BossId)
            {
                throw new Exception("You're not a boss of this agent. You cannot give him orders.");
            }

            if (agent.State != AgentState.Active)
            {
                throw new Exception("Agent " + request.AgentId + " is not in the active state");
            }

            if (request.Path == null || request.Path.Length==0)
            {
                throw new Exception("Path not provided");
            }

            //road continuity
            for (int i = 0; i < request.Path.Length - 1; i++)
            {
                if (!_mapUtils.IsAdjacent(request.Path[i], request.Path[i+1]))
                {
                    throw new Exception("Path is not continuous");
                }
            }

            var missionMapElement = await _unitOfWork.MapElements.GetByIdAsync(mission.MapElementId);
            var hq = await _unitOfWork.Headquarters.GetByBossId(agent.BossId.Value);
            var hqMapElement = await _unitOfWork.MapElements.GetByIdAsync(hq.MapElementId);

            if (!_mapUtils.IsAdjacent(request.Path[0], missionMapElement.Position))
            {
                if(_mapUtils.IsAdjacent(request.Path[^1], missionMapElement.Position))
                {
                    request.Path = request.Path.Reverse().ToArray();
                }
                else
                {
                    throw new Exception("First (or last) element of a path is not adjacent to the mission");
                }
            }

            if (!_mapUtils.IsAdjacent(request.Path[^1], hqMapElement.Position))
            {
                throw new Exception("First (or last) element of a path is not adjacent to the headquarters");
            }

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

            if (agent.State != AgentState.Moving)
            {
                throw new Exception("Agent " + agentId + " is not in the moving state");
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
