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
        Task<IList<MessageDTO>> GetMessagesToBoss(long bossId);
        Task<IList<MessageDTO>> GetMessagesFromBoss(long bossId);
        Task<string> GetMessageContent(long messageId);
        Task<IList<MessageDTO>> GetReports(long bossId);
        Task<IList<MessageDTO>> GetAllMessagesToInRange(long bossId, int? fromRange, int? toRange, string bossNameFilter, bool onlyUnseen);
        Task<IList<MessageDTO>> GetAllReportsToInRange(long bossId, int? fromRange, int? toRange, bool onlyUnseen);
        void DeleteMessage(long messageId);
        void DeleteMessages(string messageIds);
        long CountMessages(long bossId);
        long CountReports(long bossId);
        Task SetSeen(long messageId);

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
        public async Task<IList<MessageDTO>> GetMessagesToBoss(long bossId)
        {
            var messages = await _unitOfWork.Messages.GetMessagesToBoss(bossId);
            return _mapper.Map<IList<MessageDTO>>(messages);
        }

        /// <summary>
        /// Returns messages sent by boss with id=bossId
        /// </summary>
        public async Task<IList<MessageDTO>> GetMessagesFromBoss(long bossId)
        {
            var messages = await _unitOfWork.Messages.GetMessagesFromBoss(bossId);
            return _mapper.Map<IList<MessageDTO>>(messages);
        }

        public async Task<string> GetMessageContent(long messageId)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(messageId);
            if (message == null)
                throw new Exception("Message not found");
            message.Seen = true;
            _unitOfWork.Commit();
            return _mapper.Map<MessageDTO>(message).Content;
        }

        public async Task<IList<MessageDTO>> GetReports(long bossId)
        {
            var messages = await _unitOfWork.Messages.GetReports(bossId);
            return _mapper.Map<IList<MessageDTO>>(messages);
        }

        public async Task<IList<MessageDTO>> GetAllMessagesToInRange(long bossId, int? fromRange, int? toRange, string bossNameFilter, bool onlyUnseen)
        {
            var messages = await _unitOfWork.Messages.GetAllMessagesToInRange(bossId, fromRange, toRange, bossNameFilter, onlyUnseen);
            return _mapper.Map<IList<MessageDTO>>(messages);
        }

        public async Task<IList<MessageDTO>> GetAllReportsToInRange(long bossId, int? fromRange, int? toRange, bool onlyUnseen)
        {

            var reports = await _unitOfWork.Messages.GetAllReportsToInRange( bossId,fromRange,toRange, onlyUnseen);
            return _mapper.Map<IList<MessageDTO>>(reports);
        }

        public void DeleteMessage(long messageId)
        {
            _unitOfWork.Messages.DeleteById(messageId);
            _unitOfWork.Commit();
        }

        public void DeleteMessages(string messageIds)
        {
            var ids = messageIds.Split('i').Select(x => long.Parse(x)).ToArray();
            _unitOfWork.Messages.DeleteByIds(ids);
            _unitOfWork.Commit();
        }

        public long CountMessages(long bossId)
        {
            return _unitOfWork.Messages.CountMessages(bossId);
        }

        public long CountReports(long bossId)
        {
            return _unitOfWork.Messages.CountMessages(bossId);
        }

        public async Task SetSeen(long messageId)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(messageId);
            message.Seen = true;
            _unitOfWork.Commit();
        }
    }
}
