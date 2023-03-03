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
    public interface IRoleRepository : ICrudRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);
    }

    public class RoleRepository : CrudRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {

        }

        public Task<Role> GetByNameAsync(string name)
        {
            return _context.Roles.FirstOrDefaultAsync(x=>x.Name == name);
        }
    }
}
