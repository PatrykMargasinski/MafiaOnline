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
    public interface IAmbushValidator
    {
        Task ValidateMoveToArrangeAmbush(ArrangeAmbushRequest request);
        Task ValidateArrangeAmbush(ArrangeAmbushRequest request);
        Task ValidateAttackAmbush(AttackAmbushRequest request);
        Task ValidateMoveOnAttackAmbush(AttackAmbushRequest request);
        Task ValidateCancelAmbush(CancelAmbushRequest request);
    }

    public class AmbushValidator : IAmbushValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapUtils _mapUtils;

        public AmbushValidator(IUnitOfWork unitOfWork, IMapper mapper, IMapUtils mapUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapUtils = mapUtils;
        }

        public async Task ValidateMoveToArrangeAmbush(ArrangeAmbushRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            if (agent == null)
                throw new Exception("Agent not found");
            if (agent.BossId == null)
                throw new Exception("Agent has no boss");
            if (agent.BossId != request.BossId)
                throw new Exception("It's not your agent. You cannot give him orders.");
            if (agent.StateIdEnum != AgentState.Active)
                throw new Exception("Agent is not active");
            if (!_mapUtils.IsRoad(request.Point.X, request.Point.Y))
                throw new Exception("The ambush place is not on the road");
            if (_mapUtils.IsCrossroad(request.Point.X, request.Point.Y))
                throw new Exception("The ambush place cannot be placed on the crossroad");
        }

        public async Task ValidateArrangeAmbush(ArrangeAmbushRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            if (agent == null)
                throw new Exception("Agent not found");
            if (agent.BossId == null)
                throw new Exception("Agent has no boss");
            if (!_mapUtils.IsRoad(request.Point.X, request.Point.Y))
                throw new Exception("The ambush place is not on the road");
            if (_mapUtils.IsCrossroad(request.Point.X, request.Point.Y))
                throw new Exception("The ambush place cannot be placed on the crossroad");

            var mapElement = await _unitOfWork.MapElements.GetInPoint(request.Point.X, request.Point.Y);

            if (mapElement != null)
                throw new Exception("There is some map element in this point");
        }

        public async Task ValidateAttackAmbush(AttackAmbushRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(request.MapElementId);
            if (agent == null)
                throw new Exception("Agent not found");
            if (agent.BossId == null)
                throw new Exception("Agent has no boss");
            if (mapElement == null)
                throw new Exception("Map element not found. It's possible ambush was cancelled during your agent's move");
            if (mapElement.Type != MapElementType.Ambush)
                throw new Exception("This map element is not an ambush");
            if (mapElement.BossId == agent.BossId)
                throw new Exception("You cannot attack your own ambush");
        }

        public async Task ValidateMoveOnAttackAmbush(AttackAmbushRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(request.MapElementId);
            if (agent == null)
                throw new Exception("Agent not found");
            if (agent.StateIdEnum != AgentState.Active)
                throw new Exception("Agent is not active");
            if (agent.BossId == null)
                throw new Exception("Agent has no boss");
            if (agent.BossId != request.BossId)
                throw new Exception("It's not your agent. You cannot give him orders.");
            if (mapElement == null)
                throw new Exception("Map element not found");
            if (mapElement.Type != MapElementType.Ambush)
                throw new Exception("This map element is not an ambush");
            if (mapElement.BossId == agent.BossId)
                throw new Exception("You cannot attack your own ambush");
        }

        public async Task ValidateCancelAmbush(CancelAmbushRequest request)
        {
            var ambush = await _unitOfWork.Ambushes.GetByMapElementIdAsync(request.MapElementId);
            var agent = await _unitOfWork.Agents.GetByIdAsync(ambush.AgentId);
            if (ambush == null)
                throw new Exception("This map element is not an ambush.");
            if (agent.BossId != request.BossId)
                throw new Exception("It's not your agent. You cannot give him orders.");
        }
    }
}
