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
    public interface IMissionRepository : ICrudRepository<Mission>
    {
        Task<IList<Mission>> GetAvailableMissions();
        Task<IList<PerformingMission>> GetPerformingMissions(long bossId);
    }

    public class MissionRepository : CrudRepository<Mission>, IMissionRepository
    {
        public MissionRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<Mission>> GetAvailableMissions()
        {
            var missions = await _context.Missions
                .Where(x => x.State == MissionState.Available)
                .ToListAsync();
            return missions;
        }

        public async Task<IList<PerformingMission>> GetPerformingMissions(long bossId)
        {
            var missions = await _context.PerformingMissions
                .Include(x => x.Agent)
                .Include(y => y.Mission)
                .Where(z => z.Agent.BossId == bossId)
                .ToListAsync();
            return missions;
        }
    }
}
