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
    public interface IPlayerRepository : ICrudRepository<Player>
    {
        Task<Player> GetByNick(string nick);
    }

    public class PlayerRepository : CrudRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(DataContext context) : base(context)
        {

        }

        public async Task<Player> GetByNick(string nick)
        {
            var player = await _context
                .Players
                .FirstOrDefaultAsync(x => x.Nick == nick);
            return player;
        }
    }
}
