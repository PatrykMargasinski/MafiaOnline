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
    public interface IStateRepository : ICrudRepository<State>
    {
    }

    public class StateRepository : CrudRepository<State>, IStateRepository
    {
        public StateRepository(DataContext context) : base(context)
        {

        }
    }
}
