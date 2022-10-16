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
    public interface IPerformingMissionRepository : ICrudRepository<PerformingMission>
    {
        Task<PerformingMission> GetByAgentId(long agentId);
        Task<IList<PerformingMission>> GetByAgentIds(long[] agentsId);
    }

    public class PerformingMissionRepository : CrudRepository<PerformingMission>, IPerformingMissionRepository
    {
        public PerformingMissionRepository(DataContext context) : base(context)
        {

        }
        public async Task<PerformingMission> GetByAgentId(long agentId)
        {
            var pm = await _context.PerformingMissions.Where(x => agentId == x.AgentId).FirstOrDefaultAsync();
            return pm;
        }

        public async Task<IList<PerformingMission>> GetByAgentIds(long[] agentsId)
        {
            var pms = await _context.PerformingMissions.Where(x => agentsId.Contains(x.AgentId)).ToListAsync();
            return pms;
        }
    }
}
