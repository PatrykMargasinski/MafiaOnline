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
    public interface IBossRepository : ICrudRepository<Boss>
    {
        Task<IList<Boss>> GetBestBosses(int from, int to);
        Task<IList<Boss>> GetByLastName(string lastName);
        Task<Boss> GetByFullName(string fullName);
        Task<IList<string>> GetSimilarBossFullNames(string startingWithString);
        Task<IList<Boss>> GetAllWithPlayer();
    }

    public class BossRepository : CrudRepository<Boss>, IBossRepository
    {
        public BossRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<Boss>> GetBestBosses(int from, int to)
        {
            var bosses = await _context.Bosses
                .OrderByDescending(x => x.Money)
                .Skip(from)
                .Take(to - from)
                .ToListAsync();
            return bosses;
        }

        public async Task<IList<Boss>> GetByLastName(string lastName)
        {
            var bosses = await _context.Bosses
                .Where(x => x.LastName == lastName)
                .ToListAsync();
            return bosses;
        }

        public async Task<Boss> GetByFullName(string fullName)
        {
            var boss = await _context.Bosses
                .Where(x => ((x.FirstName + x.LastName).Replace(" ", string.Empty) == fullName.Replace(" ", string.Empty)) 
                || ((x.LastName + x.FirstName).Replace(" ", string.Empty) == fullName.Replace(" ", string.Empty)))
                .FirstOrDefaultAsync();
            return boss;
        }

        public async Task<IList<string>> GetSimilarBossFullNames(string startingWithString)
        {
            var bossNames = await _context.Bosses
                .Where(x => (x.FirstName + " " + x.LastName).StartsWith(startingWithString) ||
                 (x.LastName + " " + x.FirstName).StartsWith(startingWithString))
                .Select(x => x.FirstName + " " + x.LastName)
                .ToListAsync();
            return bossNames;
        }

        public async Task<IList<Boss>> GetAllWithPlayer()
        {
            return await _context.Bosses
                .Include(x=> x.Player)
                .ToListAsync();
        }
    }
}
