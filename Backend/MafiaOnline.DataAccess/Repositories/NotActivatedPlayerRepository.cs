using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface INotActivatedPlayerRepository : ICrudRepository<NotActivatedPlayer>
    {
        Task<NotActivatedPlayer> GetByPlayerId(long playerId);
        Task<NotActivatedPlayer> GetByCode(string code);
    }

    public class NotActivatedPlayerRepository : CrudRepository<NotActivatedPlayer>, INotActivatedPlayerRepository
    {
        public NotActivatedPlayerRepository(DataContext context) : base(context)
        {

        }

        public async Task<NotActivatedPlayer> GetByCode(string code)
        {
            var player = await _context
                .NotActivatedPlayers
                .FirstOrDefaultAsync(x => x.ActivationCode == code);
            return player;
        }

        public async Task<NotActivatedPlayer> GetByPlayerId(long playerId)
        {
            var player = await _context
                .NotActivatedPlayers
                .FirstOrDefaultAsync(x => x.PlayerId == playerId);
            return player;
        }
    }
}
