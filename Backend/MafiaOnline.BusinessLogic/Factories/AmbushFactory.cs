﻿
using FluentAssertions.Execution;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MafiaOnline.BusinessLogic.Factories
{


    public interface IAmbushFactory
    {
        Task<MovingAgent> CreateAgentMovingOnAmbush(ArrangeAmbushRequest request);
        Task<MovingAgent> CreateMovingAgentForAttackingAmbush(AttackAmbushRequest request);
    }

    public class AmbushFactory : IAmbushFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRandomizer _randomizer;
        private readonly IMapUtils _mapUtils;

        public AmbushFactory(IUnitOfWork unitOfWork, IRandomizer randomizer, IMapUtils mapUtils)
        {
            _unitOfWork = unitOfWork;
            _randomizer = randomizer;
            _mapUtils = mapUtils;
        }

        public async Task<MovingAgent> CreateAgentMovingOnAmbush(ArrangeAmbushRequest request)
        {
            var movingAgent = new MovingAgent()
            {
            };

            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(agent.BossId.Value);
            var headquarters = await _unitOfWork.Headquarters.GetByBossId(boss.Id);

            movingAgent.ArrivalTime = DateTime.Now
                .AddSeconds((Math.Abs(headquarters.MapElement.X - request.Point.X) + Math.Abs(headquarters.MapElement.Y - request.Point.Y)) * MapConsts.SECONDS_TO_MAKE_ONE_STEP);
            movingAgent.DestinationPoint = new Point(request.Point.X, request.Point.Y);
            movingAgent.DestinationDescription = $"Going to arrange ambush";
            movingAgent.DatasJson = JsonSerializer.Serialize(request);

            movingAgent.Agent = agent;
            agent.StateIdEnum = AgentState.Moving;
            return movingAgent;
        }

        public async Task<MovingAgent> CreateMovingAgentForAttackingAmbush(AttackAmbushRequest request)
        {
            var movingAgent = new MovingAgent()
            {
            };

            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(request.MapElementId);
            var hq = await _unitOfWork.Headquarters.GetByBossId(agent.BossId.Value);
            movingAgent.ArrivalTime = DateTime.Now
                .AddSeconds((Math.Abs(mapElement.X - hq.MapElement.X) + Math.Abs(mapElement.Y - hq.MapElement.Y)) * MapConsts.SECONDS_TO_MAKE_ONE_STEP);
            movingAgent.DestinationPoint = new Point(mapElement.X, mapElement.Y);
            movingAgent.DestinationDescription = $"Attacking ambush";
            movingAgent.DatasJson = JsonSerializer.Serialize(request);

            movingAgent.Agent = agent;
            agent.StateIdEnum = AgentState.Moving;
            return movingAgent;
        }
    }
}
