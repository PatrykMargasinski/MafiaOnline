using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Validators
{
    public interface IMessageValidator
    {
        Task ValidateSendMessage(SendMessageRequest request);
        Task ValidateDeleteMessages(long[] messageIds, long bossId);
        Task ValidateSetSeen(SetSeenRequest request);
    }

    public class MessageValidator : IMessageValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageValidator(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ValidateSendMessage(SendMessageRequest request)
        {
            var toBoss = await _unitOfWork.Bosses.GetByFullName(request.ToBossFullName);

            if (request.BossId != request.FromBossId)
                throw new Exception("You cannot send this message. You are not assigned as its sender.");

            if (toBoss == null)
                throw new Exception("Recipient not found");

            if (string.IsNullOrEmpty(request.Subject))
                throw new Exception("Subject cannot be empty");

            if (string.IsNullOrEmpty(request.Content))
                throw new Exception("Content cannot be empty");
        }

        public async Task ValidateDeleteMessages(long[] messageIds, long bossId)
        {
            var messages = await _unitOfWork.Messages.GetByIdsAsync(messageIds);
            foreach (var message in messages)
            {
                if (message.ToBossId != bossId)
                {
                    throw new Exception($"Message: \"{message.Subject}\". You cannot remove it, because you're not a recipient of this message");
                }
            }
        }

        public async Task ValidateSetSeen(SetSeenRequest request)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(request.MessageId);
            if (request.BossId != message.ToBossId)
                throw new Exception($"You cannot see this message, because you're not a recipient of this message");
        }
    }
}
