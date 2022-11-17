using AutoFixture.NUnit3;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using MafiaOnline.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Pose;
using AutoFixture;
using AutoFixture.AutoMoq;
using MafiaOnline.Test.Attributes;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.Test.Database;
using MafiaAPI.Jobs;
using AutoMapper;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.BusinessLogic.Factories;
using Quartz;
using Microsoft.Extensions.Logging;
using MafiaOnline.BusinessLogic;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Const;

namespace MafiaOnline.Test.Repositories
{
    public class MissionServiceTest
    {
        private IMapper _mapper;
        private FakeUnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new FakeUnitOfWork();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    var securityUtils = new Mock<ISecurityUtils>();
                    var missionUtils = new Mock<IMissionUtils>();
                    mc.AddProfile(new AutoMapperProfile(missionUtils.Object, securityUtils.Object));
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_GetAvailableMissions_Works_Properly(Mock<ISchedulerFactory> scheduler, Mock<IMissionUtils> missionUtils, 
            Mock<IReporter> reporter, Mock<IMissionValidator> missionValidator, Mock<IPerformMissionJobRunner> performMissionJobRunner, Mock<ILogger<MissionService>> logger, 
            Mock<IMissionFactory> missionFactory, Mock<IMissionRefreshJobRunner> missionRefreshJobRunner)
        {
            MissionService sut = new MissionService(_unitOfWork, _mapper, scheduler.Object, missionUtils.Object, reporter.Object, 
                missionValidator.Object, performMissionJobRunner.Object, logger.Object, missionFactory.Object, missionRefreshJobRunner.Object);

            var missions = await sut.GetAvailableMissions();
            Assert.AreEqual(missions.Count, 7);
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_GetPerformingMissions_Works_Properly(Mock<ISchedulerFactory> scheduler, Mock<IMissionUtils> missionUtils,
            Mock<IReporter> reporter, Mock<IMissionValidator> missionValidator, Mock<IPerformMissionJobRunner> performMissionJobRunner, Mock<ILogger<MissionService>> logger,
            Mock<IMissionFactory> missionFactory, Mock<IMissionRefreshJobRunner> missionRefreshJobRunner)
        {
            MissionService sut = new MissionService(_unitOfWork, _mapper, scheduler.Object, missionUtils.Object, reporter.Object,
                missionValidator.Object, performMissionJobRunner.Object, logger.Object, missionFactory.Object, missionRefreshJobRunner.Object);

            var missions = await sut.GetPerformingMissions(1);
            Assert.AreEqual(missions.Count, 0);
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_DoMission_Works_Properly(Mock<ISchedulerFactory> scheduler, Mock<IMissionUtils> missionUtils,
            Mock<IReporter> reporter, Mock<IMissionValidator> missionValidator, Mock<IPerformMissionJobRunner> performMissionJobRunner, Mock<ILogger<MissionService>> logger,
            Mock<IMissionFactory> missionFactory, Mock<IMissionRefreshJobRunner> missionRefreshJobRunner)
        {
            MissionService sut = new MissionService(_unitOfWork, _mapper, scheduler.Object, missionUtils.Object, reporter.Object,
                missionValidator.Object, performMissionJobRunner.Object, logger.Object, missionFactory.Object, missionRefreshJobRunner.Object);

            await sut.DoMission(1, 1);
            var performingMissions = await sut.GetPerformingMissions(1);
            Assert.AreEqual(performingMissions.Count, 1);
            Assert.AreEqual(performingMissions[0].MissionId, 1);
            Assert.AreEqual(performingMissions[0].AgentId, 1);

            var agent = await _unitOfWork.Agents.GetByIdAsync(1);
            Assert.AreEqual(agent.State, AgentState.OnMission);

            var mission = await _unitOfWork.Missions.GetByIdAsync(1);
            Assert.AreEqual(mission.State, MissionState.Performing);
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_RefreshMissions_Works_Properly(Mock<ISchedulerFactory> scheduler, Mock<IMissionUtils> missionUtils,
            Mock<IReporter> reporter, Mock<IMissionValidator> missionValidator, Mock<IPerformMissionJobRunner> performMissionJobRunner, Mock<ILogger<MissionService>> logger,
            Mock<IMissionRefreshJobRunner> missionRefreshJobRunner)
        {
            var missionFactory = new MissionFactory(_unitOfWork, new Randomizer());
            MissionService sut = new MissionService(_unitOfWork, _mapper, scheduler.Object, missionUtils.Object, reporter.Object,
                missionValidator.Object, performMissionJobRunner.Object, logger.Object, missionFactory, missionRefreshJobRunner.Object);

            await sut.RefreshMissions();

            var missions = await _unitOfWork.Missions.GetAllAsync();
            Assert.AreEqual(MissionConsts.MAX_NUMBER_OF_MISSIONS, missions.Count);
        }
    }
}
