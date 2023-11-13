
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


    public interface IMissionFactory
    {
        Task<Mission> CreateByMissionTemplate(MissionTemplate template);
        Mission Create(string name = "Mission with no name", int? difficultyLevel = null, double? duration = null, int? loot = null,
        int? strengthPercentage = null, int? dexterityPercentage = null, int? intelligencePercentage = null);
        Task<MovingAgent> CreateAgentMovingOnMission(StartMissionRequest request);
    }

    public class MissionFactory : IMissionFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRandomizer _randomizer;
        private readonly IMapUtils _mapUtils;

        public MissionFactory(IUnitOfWork unitOfWork, IRandomizer randomizer, IMapUtils mapUtils)
        {
            _unitOfWork = unitOfWork;
            _randomizer = randomizer;
            _mapUtils = mapUtils;
        }

        public async Task<Mission> CreateByMissionTemplate(MissionTemplate template)
        {
            if(template == null)
                template = await _unitOfWork.MissionTemplates.GetRandomAsync();
            var mission = Create(name: template.Name, difficultyLevel: _randomizer.Next(template.MinDifficulty, template.MaxDifficulty),
                duration: _randomizer.Next(template.MinDuration, template.MaxDuration), loot: (int)Math.Round(_randomizer.Next(template.MinLoot, template.MaxLoot)/100d)*100,
                strengthPercentage: template.StrengthPercentage, dexterityPercentage: template.DexterityPercentage, intelligencePercentage: template.IntelligencePercentage);
            return mission;
        }

        public Mission Create(string name = "Mission with no name", int? difficultyLevel = null, double? duration = null, int? loot = null, 
            int? strengthPercentage = null, int? dexterityPercentage = null, int? intelligencePercentage = null)
        {
            int strPerc, dexPerc, intPerc;
            if(!strengthPercentage.HasValue || !dexterityPercentage.HasValue || !intelligencePercentage.HasValue)
            {
                strPerc = _randomizer.Next(0, 20);
                dexPerc = _randomizer.Next(0, 20 - strPerc);
                intPerc = 20 - strPerc - dexPerc;
                strPerc *= 5; dexPerc *= 5; intPerc *= 5;
            }
            else
            {
                strPerc = strengthPercentage.Value; dexPerc = dexterityPercentage.Value; intPerc = intelligencePercentage.Value;
            }
            var mission = new Mission()
            {
                Name = name,
                DifficultyLevel = difficultyLevel ?? _randomizer.Next(1, 10),
                Duration = duration ?? _randomizer.Next(1, 10)*10,
                Loot = loot ?? _randomizer.Next(1, 10) * 100,
                StrengthPercentage = strPerc,
                DexterityPercentage = dexPerc,
                IntelligencePercentage = intPerc
            };

            var missionPosition = _mapUtils.GetNewMissionPosition().Result;
            var mapElement = new MapElement() { X = missionPosition.X, Y = missionPosition.Y, Type = MapElementType.Mission, Description = $"Mission: {mission.Name}" };
            mission.MapElement = mapElement;

            return mission;
        }

        public async Task<MovingAgent> CreateAgentMovingOnMission(StartMissionRequest request)
        {
            var movingAgent = new MovingAgent()
            {
            };

            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var mission = await _unitOfWork.Missions.GetByIdAsync(request.MissionId);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(agent.BossId.Value);
            var headquarters = await _unitOfWork.Headquarters.GetByBossId(boss.Id);
            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(mission.MapElementId);

            movingAgent.ArrivalTime = DateTime.Now
                .AddSeconds((Math.Abs(headquarters.MapElement.X - mapElement.X) + Math.Abs(headquarters.MapElement.Y - mapElement.Y)) * MapConsts.SECONDS_TO_MAKE_ONE_STEP);
            movingAgent.DestinationPoint = new Point(mapElement.X, mapElement.Y);
            movingAgent.DestinationDescription = $"Going on mission: {mission.Name}";
            movingAgent.Path = request.Path;
            movingAgent.DatasJson = JsonSerializer.Serialize(request);

            movingAgent.Agent = agent;
            agent.StateIdEnum = AgentState.Moving;
            agent.SubstateIdEnum = AgentSubstate.MovingOnMission;
            return movingAgent;
        }
    }
}
