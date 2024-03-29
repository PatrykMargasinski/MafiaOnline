﻿using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
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
        Task<MapFieldDTO[]> GenerateMap(long x, long y, long size, long bossId);
        Task<long[]> GetEdgeForBoss(long bossId);
    }

    public class MapService : IMapService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapUtils _mapUtils;

        public MapService(IUnitOfWork unitOfWork, IMapper mapper, IMapUtils mapUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapUtils = mapUtils;
        }

        public async Task<long[]> GetEdgeForBoss(long bossId)
        {
            var headquarters = await _unitOfWork.Headquarters.GetByBossId(bossId);
            var edge = new long[] {headquarters.MapElement.X - 10, headquarters.MapElement.Y - 10};
            return edge;
        }

        public async Task<MapFieldDTO[]> GenerateMap(long x, long y, long size, long bossId)
        {
            var mapElements = await _unitOfWork.MapElements.GetInRange(x, x + size, y, y + size, bossId);

            var map = new MapFieldDTO[size * size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    //set terrain
                    if (_mapUtils.IsRoad(i + x, j + y))
                    {
                        map[i*size+j] = new MapFieldDTO { X = (i + x), Y = (j + y), TerrainType = TerrainType.Road};
                    }
                    else
                    {
                        map[i*size+j] = new MapFieldDTO { X = (i + x), Y = (j + y), TerrainType = TerrainType.BuildUpArea };
                    }

                    //set map element
                    var mapElement = mapElements.FirstOrDefault(el => el.X == (i + x) && el.Y == (j + y));
                    if (mapElement != null)
                    {
                        map[i * size + j].MapElementType = mapElement.Type;
                        map[i * size + j].Id = mapElement.Id;
                        map[i * size + j].Owner = mapElement.BossId;
                        map[i * size + j].Description = mapElement.Description;
                    }
                    else
                    {
                        map[i * size + j].MapElementType = MapElementType.None;
                    }
                }
            }
            return map;
        }
    }
}
