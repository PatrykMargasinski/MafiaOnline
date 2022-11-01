using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface IMessageRepository : ICrudRepository<Message>
    {
        Task<IList<Message>> GetMessagesToBoss(long bossId);
        Task<IList<Message>> GetMessagesFromBoss(long bossId);
        Task<IList<Message>> GetReports(long bossId);
        Task<IList<Message>> GetAllMessagesToInRange(long bossId, int? fromRange, int? toRange, string bossNameFilter, bool onlyUnseen);
        Task<IList<Message>> GetAllReportsToInRange(long bossId, int? fromRange, int? toRange, bool onlyUnseen);
        long CountMessages(long bossId);
        long CountReports(long bossId);
    }

    public class MessageRepository : CrudRepository<Message>, IMessageRepository
    {
        public MessageRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<Message>> GetMessagesToBoss(long bossId)
        {
            var messages = await _context
                .Messages
                .Where(x => x.ToBossId == bossId && x.Type==MessageType.BossMessage)
                .ToListAsync();
            return messages;
        }

        public async Task<IList<Message>> GetMessagesFromBoss(long bossId)
        {
            var messages = await _context
                .Messages
                .Where(x => x.FromBossId == bossId && x.Type == MessageType.BossMessage)
                .ToListAsync();
             return messages;
        }

        public async Task<IList<Message>> GetReports(long bossId)
        {
            var messages = await _context
                .Messages
                .Where(x => x.ToBossId == bossId && x.Type == MessageType.Report)
                .ToListAsync();
            return messages;
        }

        public async Task<IList<Message>> GetAllMessagesToInRange(long bossId, int? fromRange, int? toRange, string bossNameFilter, bool onlyUnseen)
        {
            return await _context.Messages
                .Include(x => x.ToBoss)
                .Include(x => x.FromBoss)
                .Where(mes =>
                mes.ToBossId == bossId && (
                    (mes.FromBoss.FirstName + " " + mes.FromBoss.LastName).ToLower().Contains(bossNameFilter.Trim().ToLower()) ||
                    (mes.FromBoss.LastName + " " + mes.FromBoss.FirstName).ToLower().Contains(bossNameFilter.Trim().ToLower())
                ) &&
                    (!mes.Seen || !onlyUnseen) //get only unseen messages if "onlyUnseen" is true
                    && mes.Type == MessageType.BossMessage
                )
                .OrderByDescending(x => x.ReceivedDate)
                .Skip(fromRange.Value)
                .Take(toRange.Value - fromRange.Value)
                .ToListAsync();
        }

        public async Task<IList<Message>> GetAllReportsToInRange(long bossId, int? fromRange, int? toRange, bool onlyUnseen)
        {

            if (!fromRange.HasValue || !toRange.HasValue)
            {
                fromRange = 0; toRange = 5;
            }

            return await _context.Messages
                .Include(x => x.ToBoss)
                .Where(mes =>
                mes.ToBossId == bossId &&
                    (!mes.Seen || !onlyUnseen) //get only unseen reports if "onlyUnseen" is true
                    && mes.Type == MessageType.Report
                )
                .OrderByDescending(x => x.ReceivedDate)
                .Skip(fromRange.Value)
                .Take(toRange.Value - fromRange.Value)
                .ToListAsync();
        }

        public long CountMessages(long bossId)
        {
            return _context.Messages.Where(x => x.Type == MessageType.BossMessage).Count();
        }

        public long CountReports(long bossId)
        {
            return _context.Messages.Where(x => x.Type == MessageType.Report).Count();
        }
    }
}
