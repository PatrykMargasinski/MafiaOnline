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

        public async Task<MapFieldDTO[]> GenerateMap(long x, long y, long size)
        {
            var map = new MapFieldDTO[size * size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((i + x) % 6 == 0 || (j + y) % 6 == 0)
                    {
                        map[i*size+j] = new MapFieldDTO { X = i, Y = j, ElementType = 0 };
                    }
                    else
                    {
                        map[i*size+j] = new MapFieldDTO { X = i, Y = j, ElementType = 1 };
                    }
                }
            }
            return map;
        }
    }
}
