using AutoMapper;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Factories
{
    public interface IAgentFactory
    {
        Task<Agent> Create(string firstName = null, string lastName = null, int? strength = null, int? dexterity = null, int? intelligence = null, int? upkeep = null, bool isFromBossFamily = false, AgentState startingState = AgentState.Renegate);
        Task<AgentForSale> CreateForSaleInstance(Agent agent);
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

            agentForSale.Agent = agent;
            return agentForSale;
        }
    }
}
