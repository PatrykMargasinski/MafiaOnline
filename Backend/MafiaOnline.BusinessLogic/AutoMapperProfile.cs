using AutoMapper;
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
        private readonly ISecurityUtils _securityUtils;
        public AutoMapperProfile(IMissionUtils missionUtils, ISecurityUtils securityUtils)
        {
            _missionUtils = missionUtils;
            _securityUtils = securityUtils;

            //Agent
            CreateMap<Agent, AgentDTO>();
            CreateMap<Agent, AgentOnMissionDTO>()
                .ForMember(x => x.AgentName, y => y.MapFrom(z => z.FirstName + " " + z.LastName))
                .ForMember(x => x.MissionName, y => y.MapFrom(z => z.PerformingMission.Mission.Name))
                .ForMember(x => x.SuccessChance, y => y.MapFrom(z => _missionUtils.CalculateAgentSuccessChance(z, z.PerformingMission.Mission)))
                .ForMember(x => x.CompletionTime, y => y.MapFrom(z => z.PerformingMission.CompletionTime))
                .ForMember(x => x.SecondsLeft, y => y.MapFrom(z => (long) DateTime.Now.Subtract(z.PerformingMission.CompletionTime).TotalSeconds));

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

            //Message
            CreateMap<Message, MessageNoContentDTO>()
                .ForMember(x => x.FromBossName, y => y.MapFrom(z => (z.FromBoss == null) ? string.Empty : (z.FromBoss.FirstName + " " + z.FromBoss.LastName)))
                .ForMember(x => x.ToBossName, y => y.MapFrom(z => z.ToBoss.FirstName + " " + z.ToBoss.LastName))
                .ForMember(x => x.Subject, y => y.MapFrom(z => _securityUtils.Decrypt(z.Subject)));
            CreateMap<Message, MessageDTO>()
                .ForMember(x => x.FromBossName, y => y.MapFrom(z => (z.FromBoss == null) ? string.Empty : (z.FromBoss.FirstName + " " + z.FromBoss.LastName)))
                .ForMember(x => x.ToBossName, y => y.MapFrom(z => z.ToBoss.FirstName + " " + z.ToBoss.LastName))
                .ForMember(x => x.Subject, y => y.MapFrom(z => _securityUtils.Decrypt(z.Subject)))
                .ForMember(x => x.Content, y => y.MapFrom(z => _securityUtils.Decrypt(z.Content)));
        }

        public AutoMapperProfile()
        {

        }
    }
}
