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
        Task<Agent> Create(string firstName = null, string lastName = null, int? strength = null, int? dexterity = null, int? intelligence = null, int? upkeep = null);
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

        public async Task<Agent> Create(string firstName = null, string lastName = null, int? strength = null, int? dexterity = null, int? intelligence = null, int? upkeep = null)
        {
            if(string.IsNullOrEmpty(firstName)) firstName = (await _unitOfWork.Names.GetRandomFirstName()).Text;
            if (string.IsNullOrEmpty(lastName)) lastName = (await _unitOfWork.Names.GetRandomLastName()).Text;
            var rand = new Random();
            return new Agent()
            {
                State = AgentState.Renegate,
                FirstName = firstName,
                LastName = lastName,
                Strength = strength ?? rand.Next(2, 10),
                Dexterity = dexterity ?? rand.Next(2, 10),
                Intelligence = intelligence ?? rand.Next(2, 10),
                Upkeep = upkeep ?? rand.Next(2, 10) * 10,
                IsFromBossFamily = false
            };
        }
        
        public async Task<AgentForSale> CreateForSaleInstance(Agent agent)
        {
            var random = new Random();
            var agentForSale = new AgentForSale()
            {
                AgentId = agent.Id,
                Price = random.Next(2, 10) * 1000
            };
            return agentForSale;
        }
    }
}
