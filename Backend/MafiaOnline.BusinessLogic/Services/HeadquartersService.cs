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

    public interface IHeadquartersService
    {
        Task<HeadquartersDTO> GetHeadquartersDetails(long id);
    }

    public class HeadquartersService : IHeadquartersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HeadquartersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HeadquartersDTO> GetHeadquartersDetails(long id)
        {
            var headquarters = await _unitOfWork.Headquarters.GetByIdAsync(id);
            return _mapper.Map<HeadquartersDTO>(headquarters);
        }
    }
}
