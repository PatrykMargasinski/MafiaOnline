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
    public interface INewsRepository : ICrudRepository<News>
    {
        Task<IList<News>> GetLastNews(int numberOfNews);
    }

    public class NewsRepository : CrudRepository<News>, INewsRepository
    {
        public NewsRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<News>> GetLastNews(int numberOfNews)
        {
            return await _context
                .News
                .OrderByDescending(x => x.Priority)
                .ThenByDescending(x => x.Id)
                .Take(numberOfNews)
                .ToListAsync();
        }
    }
}
