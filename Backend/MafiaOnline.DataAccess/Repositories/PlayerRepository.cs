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
        Task<Player> GetByEmail(string email);
        Task<Player> GetByResetPasswordCode(string code);
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

        public async Task<Player> GetByEmail(string email)
        {
            var player = await _context
                .Players
                .FirstOrDefaultAsync(x => x.Email == email);
            return player;
        }
        public async Task<Player> GetByResetPasswordCode(string code)
        {
            var player = await _context
                .Players
                .FirstOrDefaultAsync(x => x.ResetPasswordCode == code);
            return player;
        }

    }
}
