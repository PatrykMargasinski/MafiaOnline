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

namespace MafiaOnline.Test.Repositories
{
    public class MissionUtilsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [AutoMoqData]
        public void Check_If_Success_Chance_Is_Calculated_Properly(MissionUtils sut)
        {
            Agent agent = new Agent()
            {
                 Strength=5,
                 Dexterity=5,
                 Intelligence=10
            };

            Mission mission = new Mission()
            {
                DifficultyLevel = 10,
                StrengthPercentage = 60,
                DexterityPercentage = 20,
                IntelligencePercentage = 20
            };

            Assert.AreEqual(60, sut.CalculateAgentSuccessChance(agent, mission));
        }


        [Test]
        [AutoMoqData]
        public void Check_IsMissionSuccessfullyCompleted_Method_Predicts_Success_Correctly([Frozen] Mock<IRandomizer> randomizer, MissionUtils sut)
        {
            randomizer.Setup(x => x.Next(It.IsAny<int>())).Returns(() => 50);
            Agent agent = new Agent()
            {
                Strength = 5,
                Dexterity = 5,
                Intelligence = 10
            };

            Mission mission = new Mission()
            {
                DifficultyLevel = 10,
                StrengthPercentage = 60,
                DexterityPercentage = 20,
                IntelligencePercentage = 20
            };

            Assert.IsTrue(sut.IsMissionSuccessfullyCompleted(agent, mission));
        }

        [Test]
        [AutoMoqData]
        public void Check_IsMissionSuccessfullyCompleted_Method_Predicts_Failure_Correctly([Frozen] Mock<IRandomizer> randomizer, MissionUtils sut)
        {
            randomizer.Setup(x => x.Next(It.IsAny<int>())).Returns(() => 70);
            Agent agent = new Agent()
            {
                Strength = 5,
                Dexterity = 5,
                Intelligence = 10
            };

            Mission mission = new Mission()
            {
                DifficultyLevel = 10,
                StrengthPercentage = 60,
                DexterityPercentage = 20,
                IntelligencePercentage = 20
            };

            Assert.IsFalse(sut.IsMissionSuccessfullyCompleted(agent, mission));
        }
    }
}
