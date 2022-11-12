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
    }
}
