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
    public interface IHeadquartersRepository : ICrudRepository<Headquarters>
    {

    }

    public class HeadquartersRepository : CrudRepository<Headquarters>, IHeadquartersRepository
    {
        public HeadquartersRepository(DataContext context) : base(context)
        {

        }
    }
}
