using AutoMapper;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Factories
{
    public interface IAgentFactory
    {
        Task<Agent> Create(string firstName = null, string lastName = null, int? strength = null, int? dexterity = null, int? intelligence = null, int? upkeep = null, bool isFromBossFamily = false, AgentState startingState = AgentState.Renegate);
        Task<AgentForSale> CreateForSaleInstance(Agent agent);
        Task <MovingAgent> CreateMovingAgentForPatrolInstance(PatrolRequest request);
        Task<MovingAgent> CreateMovingAgentWithLoot(long agentId, long money, Point[] path);
    }
    public class AgentFactory : IAgentFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AgentFactory(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Agent> Create(string firstName = null, string lastName = null, int? strength = null, int? dexterity = null, int? intelligence = null, int? upkeep = null, bool isFromBossFamily = false, AgentState startingState = AgentState.Renegate)
        {
            if(string.IsNullOrEmpty(firstName)) firstName = (await _unitOfWork.Names.GetRandomFirstName()).Text;
            if (string.IsNullOrEmpty(lastName)) lastName = (await _unitOfWork.Names.GetRandomLastName()).Text;
            var rand = new Random();

            strength = strength ?? rand.Next(1, 10);
            dexterity = dexterity ?? rand.Next(1, 10);
            intelligence = intelligence ?? rand.Next(1, 10);

            if (!upkeep.HasValue)
            {
                var overallPower = strength.Value + dexterity.Value + intelligence.Value;
                upkeep = rand.Next(overallPower / 2 + 1, overallPower + 1) * 10;
            }
            

            return new Agent()
            {
                State = startingState,
                FirstName = firstName,
                LastName = lastName,
                Strength = strength.Value,
                Dexterity = dexterity.Value,
                Intelligence = intelligence.Value,
                Upkeep = upkeep.Value,
                IsFromBossFamily = isFromBossFamily
            };
        }
        
        public async Task<AgentForSale> CreateForSaleInstance(Agent agent)
        {
            var random = new Random();
            var overallPower = agent.Strength + agent.Dexterity + agent.Intelligence;
            var agentForSale = new AgentForSale()
            {
                Price = random.Next(overallPower/ 2 + 1, overallPower + 1) * 1000
            };

            if (agentForSale.Price < 1000) agentForSale.Price = 1000;
            agentForSale.StartOfSale = DateTime.Now;
            agentForSale.Agent = agent;
            agent.State = AgentState.ForSale;
            return agentForSale;
        }

        public async Task<MovingAgent> CreateMovingAgentForPatrolInstance(PatrolRequest request)
        {
            var movingAgent = new MovingAgent()
            {
            };

            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);

            movingAgent.Step = 0;
            movingAgent.Path = request.Path;
            movingAgent.DestinationDescription = "Patrolling";
            movingAgent.DatasJson = JsonSerializer.Serialize(request);
            movingAgent.ArrivalTime = DateTime.Now.AddSeconds(request.Path.Length * MapConsts.SECONDS_TO_MAKE_ONE_STEP);
            movingAgent.Agent = agent;
            agent.State = AgentState.Moving;
            return movingAgent;
        }

        public async Task<MovingAgent> CreateMovingAgentWithLoot(long agentId, long money, Point[] path)
        {
            var movingAgent = new MovingAgent()
            {
            };

            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(agent.BossId.Value);
            var hq = await _unitOfWork.Headquarters.GetByBossId(boss.Id);
            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(hq.MapElementId);
            var loot = new Loot() { Money = money };
            movingAgent.Step = 0;
            movingAgent.Path = path;
            movingAgent.DestinationDescription = "Returning with loot to headquarters";
            movingAgent.DestinationPoint = new Point(mapElement.X, mapElement.Y);
            movingAgent.ArrivalTime = DateTime.Now.AddSeconds(MapConsts.SECONDS_TO_MAKE_ONE_STEP * path.Length);
            movingAgent.DatasJson = JsonSerializer.Serialize(loot);

            movingAgent.Agent = agent;
            agent.State = AgentState.MovingWithLoot;
            return movingAgent;
        }
    }
}
