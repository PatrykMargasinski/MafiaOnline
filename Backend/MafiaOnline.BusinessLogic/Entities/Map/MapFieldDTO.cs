using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public enum TerrainType
    {
        Road,
        BuildUpArea
    }

    public class MapFieldDTO
    {
        public long? Id { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
        public TerrainType Terrain { get;set; }
        public MapElementType MapElement { get; set; }
        public string Description { get; set; }
        public long? Owner { get; set; }
    }
}
