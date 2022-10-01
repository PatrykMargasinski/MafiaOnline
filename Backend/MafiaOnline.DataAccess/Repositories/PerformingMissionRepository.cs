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

    }

    public class PerformingMissionRepository : CrudRepository<PerformingMission>, IPerformingMissionRepository
    {
        public PerformingMissionRepository(DataContext context) : base(context)
        {

        }
    }
}
