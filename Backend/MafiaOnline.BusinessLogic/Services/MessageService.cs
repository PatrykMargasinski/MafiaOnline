using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IMessageService
    {
        Task SendMessage(SendMessageRequest request);
        Task<IList<MessageNoContentDTO>> GetToBossMessages(long bossId);
        Task<IList<MessageNoContentDTO>> GetFromBossMessages(long bossId);
        Task<MessageDTO> GetMessageContent(long messageId);
    }

    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecurityUtils _securityUtils;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, ISecurityUtils securityUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityUtils = securityUtils;
        }

        public async Task SendMessage(SendMessageRequest request)
        {
            var toBoss = await _unitOfWork.Bosses.GetByFullName(request.ToBossFullName);

            if (toBoss == null)
                throw new Exception("Recipient not found");

            if (string.IsNullOrEmpty(request.Subject))
                throw new Exception("Subject cannot be empty");

            if (string.IsNullOrEmpty(request.Content))
                throw new Exception("Content cannot be empty");

            var message = new Message()
            {
                Content = _securityUtils.Encrypt(request.Content),
                Subject = _securityUtils.Encrypt(request.Subject),
                ToBossId = toBoss.Id,
                FromBossId = request.FromBossId,
                ReceivedDate = DateTime.Now,
                Seen = false
            };
            _unitOfWork.Messages.Create(message);
            _unitOfWork.Commit();
        }

        public async Task<IList<MessageNoContentDTO>> GetToBossMessages(long bossId)
        {
            var messages = await _unitOfWork.Messages.GetMessagesToBoss(bossId);
            return _mapper.Map<IList<MessageNoContentDTO>>(messages);
        }

        public async Task<IList<MessageNoContentDTO>> GetFromBossMessages(long bossId)
        {
            var messages = await _unitOfWork.Messages.GetMessagesFromBoss(bossId);
            return _mapper.Map<IList<MessageNoContentDTO>>(messages);
        }

        public async Task<MessageDTO> GetMessageContent(long messageId)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(messageId);
            if (message == null)
                throw new Exception("Message not found");
            message.Seen = true;
            _unitOfWork.Commit();
            return _mapper.Map<MessageDTO>(message);
        }
    }
}
