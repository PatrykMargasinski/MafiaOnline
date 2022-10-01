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
                .Where(x => x.ToBossId == bossId)
                .ToListAsync();
            return messages;
        }

        public async Task<IList<Message>> GetMessagesFromBoss(long bossId)
        {
            var messages = await _context
                .Messages
                .Where(x => x.FromBossId == bossId)
                .ToListAsync();
             return messages;
        }
    }
}
