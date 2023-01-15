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
    public class AgentServiceTest
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
        public void Check_If_GetActiveAgents_Returns_Correct_Number_Of_Active_Agents(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory, 
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer, 
            Mock<IPatrolJobRunner> partrolJobRunner, Mock<IReporter> reporter, Mock<IMovingAgentUtils> movingAgentUtils, Mock<IAgentUtils> agentUtils, Mock<IReturnWithLootJobRunner> returnWithLootJobRunner)
        {
            AgentService sut = new AgentService(_unitOfWork, _mapper, validator.Object, factory.Object, scheduler.Object, jobRunner.Object, logger.Object, randomizer.Object,
                partrolJobRunner.Object, reporter.Object, movingAgentUtils.Object, agentUtils.Object, returnWithLootJobRunner.Object);
            var agents = sut.GetActiveAgents(1L).Result;
            Assert.AreEqual(agents.Count, 4);
        }

        [Test]
        [AutoMoqData]
        public void Check_If_GetBossAgents_Returns_Correct_Number_Of_Boss_Agents(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory,
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer,
            Mock<IPatrolJobRunner> partrolJobRunner, Mock<IReporter> reporter, Mock<IMovingAgentUtils> movingAgentUtils, Mock<IAgentUtils> agentUtils, Mock<IReturnWithLootJobRunner> returnWithLootJobRunner)
        {
            AgentService sut = new AgentService(_unitOfWork, _mapper, validator.Object, factory.Object, scheduler.Object, jobRunner.Object, logger.Object, randomizer.Object,
                partrolJobRunner.Object, reporter.Object, movingAgentUtils.Object, agentUtils.Object, returnWithLootJobRunner.Object);
            var agents = sut.GetBossAgents(1L).Result;
            Assert.AreEqual(agents.Count, 4);
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_Dismiss_Agent_Works_Properly(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory,
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer,
            Mock<IPatrolJobRunner> partrolJobRunner, Mock<IReporter> reporter, Mock<IMovingAgentUtils> movingAgentUtils, Mock<IAgentUtils> agentUtils, Mock<IReturnWithLootJobRunner> returnWithLootJobRunner)
        {
            AgentService sut = new AgentService(_unitOfWork, _mapper, validator.Object, factory.Object, scheduler.Object, jobRunner.Object, logger.Object, randomizer.Object,
                partrolJobRunner.Object, reporter.Object, movingAgentUtils.Object, agentUtils.Object, returnWithLootJobRunner.Object);
            var request = new DismissAgentRequest()
            { AgentId = 1L };
            await sut.DismissAgent(request);
            var agents = await sut.GetBossAgents(1L);
            Assert.AreEqual(agents.Count, 3);
            var agent = await _unitOfWork.Agents.GetByIdAsync(1);
            Assert.AreEqual(agent.State, AgentState.Renegate);
            Assert.IsNull(agent.BossId);
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_Recruit_Agent_Works_Properly(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory,
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer,
            Mock<IPatrolJobRunner> partrolJobRunner, Mock<IReporter> reporter, Mock<IMovingAgentUtils> movingAgentUtils, Mock<IAgentUtils> agentUtils, Mock<IReturnWithLootJobRunner> returnWithLootJobRunner)
        {
            AgentFactory agentFactory = new AgentFactory(_unitOfWork);
            AgentService sut = new AgentService(_unitOfWork, _mapper, validator.Object, factory.Object, scheduler.Object, jobRunner.Object, logger.Object, randomizer.Object,
                partrolJobRunner.Object, reporter.Object, movingAgentUtils.Object, agentUtils.Object, returnWithLootJobRunner.Object);

            var agent = await agentFactory.Create();
            agent.AgentForSale = await agentFactory.CreateForSaleInstance(agent);
            _unitOfWork.Agents.Create(agent);
            _unitOfWork.Commit();

            var request = new RecruitAgentRequest() { AgentId = agent.Id, BossId = 1L };
            await sut.RecruitAgent(request);
            var agents = await sut.GetBossAgents(1L);
            Assert.AreEqual(agents.Count, 5);
            var recruitedAgent = await _unitOfWork.Agents.GetByIdAsync(agent.Id);
            Assert.AreEqual(recruitedAgent.State, AgentState.Active);
            Assert.AreEqual(recruitedAgent.BossId, 1L);
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_Refresh_Agents_Works_Properly(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory,
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer,
            Mock<IPatrolJobRunner> partrolJobRunner, Mock<IReporter> reporter, Mock<IMovingAgentUtils> movingAgentUtils, Mock<IAgentUtils> agentUtils, Mock<IReturnWithLootJobRunner> returnWithLootJobRunner)
        {
            AgentFactory agentFactory = new AgentFactory(_unitOfWork);
            AgentService sut = new AgentService(_unitOfWork, _mapper, validator.Object, agentFactory, scheduler.Object, jobRunner.Object, logger.Object, randomizer.Object,
                partrolJobRunner.Object, reporter.Object, movingAgentUtils.Object, agentUtils.Object, returnWithLootJobRunner.Object);
            await sut.RefreshAgents();
            var agents = await _unitOfWork.Agents.GetAllAsync();
            Assert.AreEqual(agents.Where(x => x.State == AgentState.ForSale).Count(), AgentConsts.NUMBER_OF_AGENTS_FOR_SALE);
        }
    }
}
