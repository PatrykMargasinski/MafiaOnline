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
    public interface INameRepository : ICrudRepository<Name>
    {
        Task<IList<Name>> GetFirstNames();
        Task<IList<Name>> GetLastNames();
        Task<Name> GetRandomFirstName();
        Task<Name> GetRandomLastName();
    }

    public class NameRepository : CrudRepository<Name>, INameRepository
    {
        public NameRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<Name>> GetFirstNames()
        {
            return await _context.Names
                .Where(x => x.Type == NameType.FirstName)
                .ToListAsync();
        }

        public async Task<IList<Name>> GetLastNames()
        {
            return await _context.Names
                .Where(x => x.Type == NameType.LastName)
                .ToListAsync();
        }

        public async Task<Name> GetRandomFirstName()
        {
            return await _context.Names
                .Where(x => x.Type == NameType.FirstName)
                .OrderBy(r => Guid.NewGuid())
                .Take(1)
                .FirstOrDefaultAsync(); 
        }

        public async Task<Name> GetRandomLastName()
        {
            return await _context.Names
                .Where(x => x.Type == NameType.LastName)
                .OrderBy(r => Guid.NewGuid())
                .Take(1)
                .FirstOrDefaultAsync();
        }
    }
}
