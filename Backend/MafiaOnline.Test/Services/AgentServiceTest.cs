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

namespace MafiaOnline.Test.Repositories
{
    public class AgentServiceTest
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
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
        public void Check_If_GetActiveAgents_Returns_Correct_Number_Of_Active_Agents(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory, 
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer)
        {
            var unitOfWork = new FakeUnitOfWork();
            AgentService sut = new AgentService(unitOfWork, _mapper, validator.Object, factory.Object, scheduler.Object, jobRunner.Object, logger.Object, randomizer.Object);
            var agents = sut.GetActiveAgents(1L).Result;
            Assert.AreEqual(agents.Count, 4);
        }

        [Test]
        [AutoMoqData]
        public void Check_If_GetBossAgents_Returns_Correct_Number_Of_Boss_Agents(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory,
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer)
        {
            var unitOfWork = new FakeUnitOfWork();
            AgentService sut = new AgentService(unitOfWork, _mapper, validator.Object, factory.Object, scheduler.Object, jobRunner.Object, logger.Object, randomizer.Object);
            var agents = sut.GetBossAgents(1L).Result;
            Assert.AreEqual(agents.Count, 4);
        }
    }
}
