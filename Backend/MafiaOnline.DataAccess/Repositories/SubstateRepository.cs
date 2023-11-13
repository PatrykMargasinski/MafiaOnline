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
    public interface ISubstateRepository : ICrudRepository<Substate>
    {
    }

    public class SubstateRepository : CrudRepository<Substate>, ISubstateRepository
    {
        public SubstateRepository(DataContext context) : base(context)
        {

        }
    }
}
