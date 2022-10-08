using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
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
        Task<IList<MessageNoContentDTO>> GetMessagesToBoss(long bossId);
        Task<IList<MessageNoContentDTO>> GetMessagesFromBoss(long bossId);
        Task<MessageDTO> GetMessageContent(long messageId);
        Task<IList<MessageNoContentDTO>> GetReports(long bossId);
        Task<IList<MessageNoContentDTO>> GetAllMessagesToInRange(long bossId, int? fromRange, int? toRange, string bossNameFilter, bool onlyUnseen);
        Task<IList<MessageNoContentDTO>> GetAllReportsToInRange(long bossId, int? fromRange, int? toRange, bool onlyUnseen);
    }

    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecurityUtils _securityUtils;
        private readonly IMessageValidator _messageValidator;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, ISecurityUtils securityUtils, IMessageValidator messageValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityUtils = securityUtils;
            _messageValidator = messageValidator;
        }

        /// <summary>
        /// Creates a message to the boss
        /// </summary>
        public async Task SendMessage(SendMessageRequest request)
        {
            await _messageValidator.ValidateSendMessage(request);
            var toBoss = await _unitOfWork.Bosses.GetByFullName(request.ToBossFullName);

            var message = new Message()
            {
                Content = _securityUtils.Encrypt(request.Content),
                Subject = _securityUtils.Encrypt(request.Subject),
                ToBossId = toBoss.Id,
                FromBossId = request.FromBossId,
                ReceivedDate = DateTime.Now,
                Type = MessageType.BossMessage,
                Seen = false
            };
            _unitOfWork.Messages.Create(message);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Returns list of messages without content sent to boss with id=bossId
        /// </summary>
        public async Task<IList<MessageNoContentDTO>> GetMessagesToBoss(long bossId)
        {
            var messages = await _unitOfWork.Messages.GetMessagesToBoss(bossId);
            return _mapper.Map<IList<MessageNoContentDTO>>(messages);
        }

        /// <summary>
        /// Returns messages sent by boss with id=bossId
        /// </summary>
        public async Task<IList<MessageNoContentDTO>> GetMessagesFromBoss(long bossId)
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

        public async Task<IList<MessageNoContentDTO>> GetReports(long bossId)
        {
            var messages = await _unitOfWork.Messages.GetReports(bossId);
            return _mapper.Map<IList<MessageNoContentDTO>>(messages);
        }

        public async Task<IList<MessageNoContentDTO>> GetAllMessagesToInRange(long bossId, int? fromRange, int? toRange, string bossNameFilter, bool onlyUnseen)
        {
            var messages = await _unitOfWork.Messages.GetAllMessagesToInRange(bossId, fromRange, toRange, bossNameFilter, onlyUnseen);
            return _mapper.Map<IList<MessageNoContentDTO>>(messages);
        }

        public async Task<IList<MessageNoContentDTO>> GetAllReportsToInRange(long bossId, int? fromRange, int? toRange, bool onlyUnseen)
        {

            var reports = await _unitOfWork.Messages.GetAllReportsToInRange( bossId,fromRange,toRange, onlyUnseen);
            return _mapper.Map<IList<MessageNoContentDTO>>(reports);
        }
    }
}
