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
    public interface IMapElementRepository : ICrudRepository<MapElement>
    {
        Task<IList<MapElement>> GetInRange(long xMin, long xMax, long yMin, long yMax);
        Task<MapElement> GetInPoint(long x, long y);
    }

    public class MapElementRepository : CrudRepository<MapElement>, IMapElementRepository
    {
        public MapElementRepository(DataContext context) : base(context)
        {

        }

        public async Task<MapElement> GetInPoint(long x, long y)
        {
            var mapElements = await _context.MapElements.FirstOrDefaultAsync(el => el.X == x && el.Y == y);
            return mapElements;
        }

        public async Task<IList<MapElement>> GetInRange(long xMin, long xMax, long yMin, long yMax)
        {
            var mapElements = await _context.MapElements.Where(x => x.X >= xMin && x.X < xMax && x.Y >= yMin && x.Y < yMax).ToListAsync();
            return mapElements;
        }
    }
}
