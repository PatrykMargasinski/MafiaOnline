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
    public class MessageServiceTest
    {
        private IMapper _mapper;
        private FakeUnitOfWork _unitOfWork;
        private Mock<ISecurityUtils> _securityUtils;

        [SetUp]
        public void Setup()
        {
            _securityUtils = new Mock<ISecurityUtils>();
            _securityUtils.Setup(x => x.Encrypt(It.IsAny<string>())).Returns((string a) => a);
            _unitOfWork = new FakeUnitOfWork();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {

                    var missionUtils = new Mock<IMissionUtils>();
                    mc.AddProfile(new AutoMapperProfile(missionUtils.Object, _securityUtils.Object));
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;

            }
        }

        [Test]
        [AutoMoqData]
        public async Task Check_If_SendMessage_Sends_Message(Mock<IMessageValidator> validator)
        {
            validator.Setup(x => x.ValidateSendMessage(It.IsAny<SendMessageRequest>())).Returns(Task.FromResult(true));
            MessageService sut = new MessageService(_unitOfWork, _mapper, _securityUtils.Object, validator.Object);

            var request = new SendMessageRequest() { Content = "test", FromBossId = 2, Subject = "test", ToBossFullName = "Patricio Rico" };

            await sut.SendMessage(request);

            var request2 = new SendMessageRequest() { Content = "test2", FromBossId = 2, Subject = "test2", ToBossFullName = "Patricio Rico" };

            await sut.SendMessage(request2);

            var messages = await _unitOfWork.Messages.GetAllAsync();

            Assert.AreEqual(2, messages.Where(x => x.ToBossId == 1).Count());
        }
    }
}
