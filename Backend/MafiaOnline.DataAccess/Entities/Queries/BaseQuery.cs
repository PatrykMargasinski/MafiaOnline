using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MafiaOnline.DataAccess.Entities.Queries
{
    public class BaseQuery<T>
    {
        public long BossId { get; set; }
        public string SortBy { get; set; }
        public bool SortDesc { get; set; }

        public IQueryable<T> ApplySorting(IQueryable<T> queryable)
        {
            if(!string.IsNullOrEmpty(SortBy))
            { 
                queryable = SortDesc
                    ? queryable.OrderByDescending(x => EF.Property<object>(x, SortBy))
                    : queryable.OrderBy(x => EF.Property<object>(x, SortBy));
            }
            return queryable;
        }
    }
}
