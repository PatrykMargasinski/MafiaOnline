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
    }

    public class MapElementRepository : CrudRepository<MapElement>, IMapElementRepository
    {
        public MapElementRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<MapElement>> GetInRange(long xMin, long xMax, long yMin, long yMax)
        {
            var mapElements = await _context.MapElements.Where(x => x.X >= xMin && x.X < xMax && x.Y >= yMin && x.Y < yMax).ToListAsync();
            return mapElements;
        }
    }
}
