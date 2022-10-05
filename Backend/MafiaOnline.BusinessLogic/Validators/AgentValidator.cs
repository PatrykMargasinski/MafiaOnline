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
    public interface IAgentValidator
    {
        Task ValidateAbandonAgent(long agentId);
        Task ValidateRecruitAgent(long agentId, long bossId);
    }

    public class AgentValidator : IAgentValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AgentValidator(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ValidateAbandonAgent(long agentId)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);
            if (agent == null)
                throw new Exception("Agent not found");
            if (agent.State != AgentState.Active)
                throw new Exception("Agent isn't active - he cannot be abandoned");
            if (agent.BossId == null)
                throw new Exception("Agent doesn't belong to any boss");
        }

        public async Task ValidateRecruitAgent(long bossId, long agentId)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(agentId);
            if (agent == null)
                throw new Exception("Agent not found");
            var boss = await _unitOfWork.Bosses.GetByIdAsync(bossId);
            if (agent.State != AgentState.OnSale)
                throw new Exception("Agent is not for sale");
            if (agent.AgentForSale == null)
                throw new Exception("There is no AgentForSale instance");
            if (boss == null)
                throw new Exception("Boss not found");
            if (agent.AgentForSale.Price > boss.Money)
                throw new Exception("Boss cannot afford the agent");
        }
    }
}
