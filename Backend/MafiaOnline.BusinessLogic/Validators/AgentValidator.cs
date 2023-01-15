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
    public interface IAgentValidator
    {
        Task ValidateDismissAgent(DismissAgentRequest request);
        Task ValidateRecruitAgent(RecruitAgentRequest request);
        Task ValidateSendToPatrol(PatrolRequest request);
    }

    public class AgentValidator : IAgentValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapUtils _mapUtils;

        public AgentValidator(IUnitOfWork unitOfWork, IMapper mapper, IMapUtils mapUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapUtils = mapUtils;
        }

        public async Task ValidateDismissAgent(DismissAgentRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            if (agent == null)
                throw new Exception("Agent not found");
            if (agent.State != AgentState.Active)
                throw new Exception("Agent isn't active - he cannot be abandoned");
            if (agent.BossId == null)
                throw new Exception("Agent doesn't belong to any boss");
            if (agent.IsFromBossFamily)
                throw new Exception("You cannot dismiss agent from the boss family");
        }

        public async Task ValidateRecruitAgent(RecruitAgentRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            if (agent == null)
                throw new Exception("Agent not found");
            var boss = await _unitOfWork.Bosses.GetByIdAsync(request.BossId);
            if (agent.State != AgentState.ForSale)
                throw new Exception("Agent is not for sale");
            if (agent.AgentForSale == null)
                throw new Exception("There is no AgentForSale instance");
            if (boss == null)
                throw new Exception("Boss not found");
            if (agent.AgentForSale.Price > boss.Money)
                throw new Exception("Boss cannot afford the agent");
        }

        public async Task ValidateSendToPatrol(PatrolRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            if (agent == null)
                throw new Exception("Agent not found");
            if (agent.State != AgentState.Active)
                throw new Exception("Agent is not active");
            if (agent.BossId == null)
                throw new Exception("Agent has no boss");
            var boss = await _unitOfWork.Bosses.GetByIdAsync(agent.BossId.Value);
            if (boss == null)
                throw new Exception("Boss not found");
            if (request.Path == null || request.Path.Length < 4)
                throw new Exception("Path is too short");
            var headquarters = await _unitOfWork.Headquarters.GetByBossId(boss.Id);
            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(headquarters.Id);
            var firstPointOfPath = request.Path[0];
            if (!_mapUtils.IsAdjacent(firstPointOfPath, new Point(mapElement.X, mapElement.Y)))
            {
                throw new Exception("First point of a path is not adjacent to the headquarters");
            }
        }
    }
}