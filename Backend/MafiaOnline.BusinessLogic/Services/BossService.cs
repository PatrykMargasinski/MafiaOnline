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

    public interface IBossService
    {
        Task<BossDTO> GetBossDatas(long id);
        Task<IList<BossDTO>> GetBestBosses(int from, int to);
    }

    public class BossService : IBossService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BossService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns basic informations about the boss
        /// </summary>
        public async Task<BossDTO> GetBossDatas(long id)
        {
            var boss = await _unitOfWork.Bosses.GetByIdAsync(id);
            return _mapper.Map<BossDTO>(boss);
        }

        /// <summary>
        /// Returns ranked bosses in position between "from" and "to"
        /// </summary>
        public async Task<IList<BossDTO>> GetBestBosses(int from, int to)
        {
            var bosses = await _unitOfWork.Bosses.GetBestBosses(from, to);
            return _mapper.Map<IList<BossDTO>>(bosses);
        }
    }
}
