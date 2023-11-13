using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            CreateMap<Agent, AgentDTO>()
                .ForMember(x => x.StateName, y => y.MapFrom(z => Enum.GetName(typeof(AgentState), z.StateIdEnum.Value)))
                .ForMember(x => x.SubstateName, y => y.MapFrom(z => z.SubstateId.HasValue ? Enum.GetName(typeof(AgentSubstate), z.SubstateIdEnum.Value) : null))
                .ForMember(x => x.State, y => y.MapFrom(z => z.StateIdEnum))
                .ForMember(x => x.Substate, y => y.MapFrom(z => z.SubstateIdEnum));

            CreateMap<Agent, AgentOnMissionDTO>()
                .ForMember(x => x.AgentName, y => y.MapFrom(z => z.FirstName + " " + z.LastName))
                .ForMember(x => x.MissionName, y => y.MapFrom(z => z.PerformingMission.Mission.Name))
                .ForMember(x => x.SuccessChance, y => y.MapFrom(z => _missionUtils.CalculateAgentSuccessChance(z, z.PerformingMission.Mission)))
                .ForMember(x => x.CompletionTime, y => y.MapFrom(z => z.PerformingMission.CompletionTime))
                .ForMember(x => x.SecondsLeft, y => y.MapFrom(z => (long)DateTime.Now.Subtract(z.PerformingMission.CompletionTime).TotalSeconds))
                .ForMember(x => x.MissionPosition, y => y.MapFrom(z => z.PerformingMission.Mission.MapElement.Position));

            CreateMap<Agent, AgentForSaleDTO>()
                .ForMember(x => x.Price, y => y.MapFrom(z => z.AgentForSale.Price));

            CreateMap<Agent, MovingAgentDTO>()
                .ForMember(x => x.DestinationDescription, y => y.MapFrom(z => z.MovingAgent.DestinationDescription))
                .ForMember(x => x.CurrentPosition, y => y.MapFrom(z => z.MovingAgent.CurrentPoint))
                .ForMember(x => x.DestinationPosition, y => y.MapFrom(z => z.MovingAgent.DestinationPoint))
                .ForMember(x => x.ArrivalTime, y => y.MapFrom(z => z.MovingAgent.ArrivalTime));

            CreateMap<Agent, AmbushingAgentDTO>()
                .ForMember(x => x.Position, y => y.MapFrom(z => z.Ambush.MapElement.Position))
                .ForMember(x => x.AmbushId, y => y.MapFrom(z => z.Ambush.Id))
                .ForMember(x => x.MapElementId, y => y.MapFrom(z => z.Ambush.MapElementId));

            //Ambush
            CreateMap<Ambush, AmbushDTO>()
                .ForMember(x => x.AgentFullName, y => y.MapFrom(z => z.Agent.FullName))
                .ForMember(x => x.BossLastName, y => y.MapFrom(z => z.Boss.LastName));

            //Boss
            CreateMap<Boss, BossDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.FullName));

            CreateMap<VBossWithPosition, BossWithPositionDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.FullName));

            //News
            CreateMap<News, NewsDTO>();

            //Mission
            CreateMap<Mission, MissionDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name + (z.RepeatableMission ? " (repeatable)" : "")))
                .ForMember(x => x.X, y => y.MapFrom(z => z.MapElement.X))
                .ForMember(x => x.Y, y => y.MapFrom(z => z.MapElement.Y));
            CreateMap<PerformingMission, PerformingMissionDTO>()
                .ForMember(x => x.AgentName, y => y.MapFrom(z => z.Agent.FirstName + " " + z.Agent.LastName))
                .ForMember(x => x.MissionName, y => y.MapFrom(z => z.Mission.Name))
                .ForMember(x => x.SuccessChance, y => y.MapFrom(z => _missionUtils.CalculateAgentSuccessChance(z.Agent, z.Mission)))
                .ForMember(x => x.Loot, y => y.MapFrom(z => z.Mission.Loot))
                .ForMember(x => x.SecondsLeft, y => y.MapFrom(z => (long)DateTime.Now.Subtract(z.CompletionTime).TotalSeconds))
                .ForMember(x => x.X, y => y.MapFrom(z => z.Mission.MapElement.X))
                .ForMember(x => x.Y, y => y.MapFrom(z => z.Mission.MapElement.Y));

            //Message
            CreateMap<Message, MessageDTO>()
                .ForMember(x => x.FromBossName, y => y.MapFrom(z => (z.FromBoss == null) ? string.Empty : (z.FromBoss.FirstName + " " + z.FromBoss.LastName)))
                .ForMember(x => x.ToBossName, y => y.MapFrom(z => z.ToBoss.FirstName + " " + z.ToBoss.LastName))
                .ForMember(x => x.IsReport, y => y.MapFrom(z => z.Type == MessageType.Report))
                .ForMember(x => x.Subject, y => y.MapFrom(z => _securityUtils.Decrypt(z.Subject)))
                .ForMember(x => x.Content, y => y.MapFrom(z => _securityUtils.Decrypt(z.Content)));

            //Headquarters
            CreateMap<Headquarters, HeadquartersDTO>()
                .ForMember(x => x.BossFirstName, y => y.MapFrom(z=>z.Boss.FirstName))
                .ForMember(x => x.BossLastName, y => y.MapFrom(z => z.Boss.LastName));
        }

        public AutoMapperProfile()
        {

        }
    }
}
