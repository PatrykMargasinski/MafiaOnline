using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IAgentService
    {
        Task<IList<AgentDTO>> GetAgentList();
    }

    public class AgentService : IAgentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AgentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<AgentDTO>> GetAgentList()
        {
            var agents = await _unitOfWork.Agents.GetAllAsync();
            return _mapper.Map<IList<AgentDTO>>(agents);
        }
    }
}
