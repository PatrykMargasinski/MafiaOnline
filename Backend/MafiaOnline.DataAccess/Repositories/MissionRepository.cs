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

    }

    public class MissionRepository : CrudRepository<Mission>, IMissionRepository
    {
        public MissionRepository(DataContext context) : base(context)
        {

        }
    }
}
