using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IMapUtils
    {
        bool IsCorner(long x, long y);
        bool IsStreet(long x, long y);
        bool IsRoad(long x, long y);
        List<(long, long)> GetPossibleNewHeadquartersPositionFromPoint(long x0, long y0);
    }

    public class MapUtils : IMapUtils
    {

        public MapUtils()
        {
            
        }

        public bool IsCorner(long x, long y)
        {
            return ((x % 6 == 1) && (x % 6 == 5)) || ((y % 6 == 1) && (y % 6 == 5));
        }

        public bool IsStreet(long x, long y)
        {
            return
            ((x % 6 == 1 || x % 6 == 5) && y % 6 != 0) ||
            ((y % 6 == 1 || y % 6 == 5) && x % 6 != 0);
        }

        public bool IsRoad(long x, long y)
        {
            return x % 6 == 0 || y % 6 == 0;
        }

        public List<(long, long)> GetPossibleNewHeadquartersPositionFromPoint(long x0, long y0)
        {
            var possibilities = new List<(long, long)>();
            for (long i = x0 - 15; i <= x0 + 15; i++)
                for (long j = y0 - 15; j <= y0 + 15; j++)
                    possibilities.Add((i, j));

            possibilities = possibilities
                .Where(x => x.Item1 > (x0 - 10) && x.Item1 <= (x0 + 10) && x.Item2 > (y0 - 10) && x.Item2 <= (y0 + 10))
                .Where(x => IsStreet(x.Item1, x.Item2) && !IsCorner(x.Item1,x.Item2))
                .ToList();

            return possibilities;
        }
    }
}
