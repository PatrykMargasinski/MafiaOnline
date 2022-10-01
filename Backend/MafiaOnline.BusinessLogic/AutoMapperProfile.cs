﻿using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
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
        private readonly IMissionUtils _missionUtils;
        public AutoMapperProfile(IMissionUtils missionUtils)
        {
            _missionUtils = missionUtils;

            //Agent
            CreateMap<Agent, AgentDTO>();
            CreateMap<Agent, AgentOnMissionDTO>()
                .ForMember(x => x.AgentName, y => y.MapFrom(z => z.FirstName + " " + z.LastName))
                .ForMember(x => x.MissionName, y => y.MapFrom(z => z.PerformingMission.Mission.Name))
                .ForMember(x => x.SuccessChance, y => y.MapFrom(z => _missionUtils.CalculateAgentSuccessChance(z, z.PerformingMission.Mission)));
            CreateMap<Agent, AgentForSaleDTO>()
                .ForMember(x => x.Price, y => y.MapFrom(z => z.AgentForSale.Price));

            //Boss
            CreateMap<Boss, BossDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.FirstName + " " + z.LastName));

            //Mission
            CreateMap<Mission, MissionDTO>();
            CreateMap<PerformingMission, PerformingMissionDTO>()
                .ForMember(x => x.AgentName, y => y.MapFrom(z => z.Agent.FirstName + " " + z.Agent.LastName))
                .ForMember(x => x.MissionName, y => y.MapFrom(z => z.Mission.Name))
                .ForMember(x => x.SuccessChance, y => y.MapFrom(z => _missionUtils.CalculateAgentSuccessChance(z.Agent, z.Mission)))
                .ForMember(x => x.Loot, y => y.MapFrom(z => z.Mission.Loot));
        }

        public AutoMapperProfile()
        {

        }
    }
}
