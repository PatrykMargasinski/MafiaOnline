using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IMapService
    {
        Task<MapFieldDTO[]> GenerateMap(long x, long y, long size);
        Task<long[]> GetEdgeForBoss(long bossId);
    }

    public class MapService : IMapService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MapService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<long[]> GetEdgeForBoss(long bossId)
        {
            var headquarters = await _unitOfWork.Headquarters.GetByBossId(bossId);
            var edge = new long[] {headquarters.X - 10, headquarters.Y - 10};
            return edge;
        }

        public async Task<MapFieldDTO[]> GenerateMap(long x, long y, long size)
        {
            var mapElements = await _unitOfWork.MapElements.GetInRange(x, x + size, y, y + size);

            var map = new MapFieldDTO[size * size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    //set terrain
                    if ((i + x) % 6 == 0 || (j + y) % 6 == 0)
                    {
                        map[i*size+j] = new MapFieldDTO { X = (i + x), Y = (j + y), Terrain = TerrainType.Road};
                    }
                    else
                    {
                        map[i*size+j] = new MapFieldDTO { X = (i + x), Y = (j + y), Terrain = TerrainType.BuildUpArea };
                    }

                    //set map element
                    var mapElement = mapElements.FirstOrDefault(el => el.X == (i + x) && el.Y == (j + y));
                    if (mapElement != null)
                    {
                        map[i * size + j].MapElement = mapElement.Type;
                        map[i * size + j].Id = mapElement.Id;
                        map[i * size + j].Owner = mapElement.Owner;
                        map[i * size + j].Description = mapElement.Description;
                    }
                    else
                    {
                        map[i * size + j].MapElement = MapElementType.None;
                    }
                }
            }
            return map;
        }
    }
}
