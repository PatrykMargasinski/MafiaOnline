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
    public interface IHeadquartersRepository : ICrudRepository<Headquarters>
    {
        Task<Headquarters> GetByBossId(long bossId);
        Task<Headquarters> GetByMapElementIdAsync(long id);
    }

    public class HeadquartersRepository : CrudRepository<Headquarters>, IHeadquartersRepository
    {
        public HeadquartersRepository(DataContext context) : base(context)
        {

        }

        public async override Task<Headquarters> GetByIdAsync(long id)
        {
            return await entities
                .Include(x => x.MapElement)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async override Task<IList<Headquarters>> GetByIdsAsync(long[] ids)
        {
            return await entities
                .Include(x => x.MapElement)
                .Include(x => x.Boss)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<Headquarters> GetByMapElementIdAsync(long id)
        {
            return await entities
                .Include(x => x.MapElement)
                .Include(x => x.Boss)
                .SingleOrDefaultAsync(s => s.MapElementId == id);
        }

        public async Task<Headquarters> GetByBossId(long bossId)
        {
            return await entities
                .Include(x => x.MapElement)
                .Include(x=>x.Boss)
                .FirstOrDefaultAsync(x => x.BossId == bossId);
        }
    }
}
