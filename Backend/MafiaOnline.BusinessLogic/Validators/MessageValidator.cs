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

            if (toBoss == null)
                throw new Exception("Recipient not found");

            if (string.IsNullOrEmpty(request.Subject))
                throw new Exception("Subject cannot be empty");

            if (string.IsNullOrEmpty(request.Content))
                throw new Exception("Content cannot be empty");
        }
    }
}
