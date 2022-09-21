using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Agent
            CreateMap<Agent, AgentDTO>();

            //Boss
            CreateMap<Boss, BossDTO>()
                .ForMember(x=>x.Name, y=>y.MapFrom(z=>z.FirstName+" "+z.LastName));
        }
    }
}
