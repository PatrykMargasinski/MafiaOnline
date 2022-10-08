using AutoMapper;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IReporter
    {
        Task CreateReport(long toBossId, string subject, string content);
    }

    public class Reporter : IReporter
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecurityUtils _securityUtils;

        public Reporter (IUnitOfWork unitOfWork, IMapper mapper, ISecurityUtils securityUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityUtils = securityUtils;
        }

        /// <summary>
        /// Creates a report
        /// </summary>
        public async Task CreateReport(long toBossId, string subject, string content)
        {
            var toBoss = await _unitOfWork.Bosses.GetByIdAsync(toBossId);

            if (toBoss == null)
                throw new Exception("Recipient not found");

            if (string.IsNullOrEmpty(subject))
                throw new Exception("Subject cannot be empty");

            if (string.IsNullOrEmpty(content))
                throw new Exception("Content cannot be empty");

            var message = new Message()
            {
                Content = _securityUtils.Encrypt(content),
                Subject = _securityUtils.Encrypt(subject),
                ToBossId = toBoss.Id,
                ReceivedDate = DateTime.Now,
                Type = MessageType.BossMessage,
                Seen = false
            };
            _unitOfWork.Messages.Create(message);
            _unitOfWork.Commit();
        }
    }
}
