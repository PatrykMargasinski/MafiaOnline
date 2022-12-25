using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface IAmbushRepository : ICrudRepository<Ambush>
    {
        Task<Ambush> GetByMapElementIdAsync(long id);
        Task<IList<Ambush>> GetByBossId(long bossId);

    }

    public class AmbushRepository : CrudRepository<Ambush>, IAmbushRepository
    {
        public AmbushRepository(DataContext context) : base(context)
        {

        }

        public async override Task<Ambush> GetByIdAsync(long id)
        {
            return await entities
                .Include(x => x.MapElement)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async override Task<IList<Ambush>> GetByIdsAsync(long[] ids)
        {
            return await entities
                .Include(x => x.MapElement)
                .Include(x => x.Boss)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<Ambush> GetByMapElementIdAsync(long id)
        {
            return await entities
                .Include(x => x.MapElement)
                .Include(x => x.Boss)
                .Include(x => x.Agent)
                .SingleOrDefaultAsync(s => s.MapElementId == id);
        }

        public async Task<IList<Ambush>> GetByBossId(long bossId)
        {
            return await entities
                .Include(x => x.MapElement)
                .Include(x => x.Boss)
                .Where(x => x.BossId == bossId)
                .ToListAsync();
        }
    }
}
