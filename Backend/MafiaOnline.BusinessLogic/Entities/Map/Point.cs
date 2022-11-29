using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities.Map
{
    public class Point
    {
        public long X { get; set; }
        public long Y { get; set; }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }   

        public override bool Equals(object other)
        {
            return X == ((Point)other).X && Y == ((Point)other).Y;
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() ^ Y.GetHashCode());
        }
    }
}
