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
    }
}
