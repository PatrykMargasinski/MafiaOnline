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

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IBossService
    {
        Task<BossWithPositionDTO> GetBossDatas(long id);
        Task<IList<BossWithPositionDTO>> GetBestBosses(int from, int to);
        Task<IList<string>> GetSimilarBossFullNames(string startingWithString);
        Task AwardingVictory();
    }

    public class BossService : IBossService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGameUtils _gameUtils;
        private readonly IReporter _reporter;

        public BossService(IUnitOfWork unitOfWork, IMapper mapper, IGameUtils gameUtils, IReporter reporter)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _gameUtils = gameUtils;
            _reporter = reporter;
        }

        /// <summary>
        /// Returns basic informations about the boss
        /// </summary>
        public async Task<BossWithPositionDTO> GetBossDatas(long id)
        {
            var boss = await _unitOfWork.VBosses.GetByIdAsync(id);
            return _mapper.Map<BossWithPositionDTO>(boss);
        }

        /// <summary>
        /// Returns ranked bosses in position between "from" and "to"
        /// </summary>
        public async Task<IList<BossWithPositionDTO>> GetBestBosses(int from, int to)
        {
            var bosses = await _unitOfWork.Bosses.GetBestBosses(from, to);
            return _mapper.Map<IList<BossWithPositionDTO>>(bosses);
        }

        /// <summary>
        /// Returns boss full names starting with "startingWithString"
        /// </summary>
        public async Task<IList<string>> GetSimilarBossFullNames(string startingWithString)
        {
            var bossNames = await _unitOfWork.Bosses.GetSimilarBossFullNames(startingWithString);
            return bossNames;
        }

        /// <summary>
        /// Awards victory for every boss and resets the game
        /// </summary>
        public async Task AwardingVictory()
        {
            var bosses = await _unitOfWork.Bosses.GetAllWithPlayer();
            bosses = bosses.OrderByDescending(x => x.Money).ToList();
            long place = 1;
            foreach(var boss in bosses)
            {
                await _reporter.CreateReport(boss.Id, "Game finished", $"You placed {place++} place. Congratulations!");
            }
            await _gameUtils.ResetGame();
        }
    }
}
