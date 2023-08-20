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
    public interface IVBossRepository : ICrudRepository<VBossWithPosition>
    {

    }

    public class VBossRepository : CrudRepository<VBossWithPosition>, IVBossRepository
    {
        public VBossRepository(DataContext context) : base(context)
        {

        }
    }
}
