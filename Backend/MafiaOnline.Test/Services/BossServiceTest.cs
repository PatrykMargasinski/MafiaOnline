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
using System.Security.Cryptography.X509Certificates;

namespace MafiaOnline.Test.Repositories
{
    public class BossServiceTest
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
        public async Task Check_If_GetBossDatas_Works_Correctly()
        {
            BossService sut = new BossService(_unitOfWork, _mapper);
            var bossDatas = await sut.GetBossDatas(1);
            Assert.IsNotNull(bossDatas);
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_GetBestBosses_Returns_Best_Bosses(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory,
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            BossService sut = new BossService(_unitOfWork, _mapper);
            for(int i=0;i<10;i++)
            {

                var newBoss = fixture.Build<Boss>().Without(p => p.Agents).Without(x=>x.MessageFromBosses).Without(x=>x.MessageToBosses).Without(x=>x.Id).Without(x=>x.Player).Create();
                newBoss.LastName = "Position"+i;
                newBoss.Money = 10000 - i*1000;
                _unitOfWork.Bosses.Create(newBoss);
            }
            _unitOfWork.Commit();

            var bestBosses = await sut.GetBestBosses(0, 5);
            Assert.AreEqual(10000, bestBosses[0].Money);
            Assert.AreEqual(9000,bestBosses[1].Money);
            Assert.AreEqual(8000,bestBosses[2].Money);
            Assert.AreEqual(7000,bestBosses[3].Money);
            Assert.AreEqual(6000,bestBosses[4].Money);
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_GetSimilarBossFullNames_Returns_Correct_FullNames(Mock<IAgentValidator> validator, Mock<IAgentFactory> factory,
            Mock<ISchedulerFactory> scheduler, Mock<IAgentRefreshJobRunner> jobRunner, Mock<ILogger<AgentService>> logger, Mock<IRandomizer> randomizer)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            BossService sut = new BossService(_unitOfWork, _mapper);

            var someLastNames = new string[] { "Melody", "Melster", "Medusa", "Meluse" };

            var someFirstNames = new string[] { "Melo", "Med", "Me", "Peter" };

            foreach (var lastName in someLastNames)
            {

                var newBoss = fixture.Build<Boss>().Without(p => p.Agents).Without(x => x.MessageFromBosses).Without(x => x.MessageToBosses).Without(x => x.Id).Without(x => x.Player).Create();
                newBoss.LastName = lastName;
                _unitOfWork.Bosses.Create(newBoss);
            }

            foreach (var firstName in someFirstNames)
            {

                var newBoss = fixture.Build<Boss>().Without(p => p.Agents).Without(x => x.MessageFromBosses).Without(x => x.MessageToBosses).Without(x => x.Id).Without(x => x.Player).Create();
                newBoss.FirstName = firstName;
                _unitOfWork.Bosses.Create(newBoss);
            }
            _unitOfWork.Commit();

            var similarNames = await sut.GetSimilarBossFullNames("Mel");
            Assert.AreEqual(4, similarNames.Count);
        }
    }
}
