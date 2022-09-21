using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface ICrudRepository<T> where T : Entity
    {

        public Task<IList<T>> GetAllAsync();
        public Task<T> GetByIdAsync(long id);
        public Task<IList<T>> GetByIdsAsync(long[] ids);
        public Task<T> GetRandomAsync();
        public long Create(T model);
        public void Update(T model);
        public void DeleteById(long id);
        public void DeleteByIds(long[] ids);
        public bool Exists(long id);

    }
    public abstract class CrudRepository<T> : ICrudRepository<T> where T : Entity
    {
        protected readonly DataContext _context;
        protected DbSet<T> entities;
        public CrudRepository(DataContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }

        public async virtual Task<IList<T>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }
        public async virtual Task<T> GetByIdAsync(long id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }

        public async virtual Task<IList<T>> GetByIdsAsync(long[] ids)
        {
            return await entities.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async virtual Task<T> GetRandomAsync()
        {
            return await entities.OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync();
        }

        public virtual long Create(T model)
        {
            entities.Add(model);
            return model.Id;
        }

        public virtual void Update(T model)
        {
            entities.Update(model);
        }

        public virtual void DeleteById(long id)
        {
            entities.Remove(GetByIdAsync(id).Result);
        }

        public virtual void DeleteByIds(long[] ids)
        {
            entities.RemoveRange(entities.Where(x => ids.Contains(x.Id)));
        }
        public virtual bool Exists(long id)
        {
            var entity = entities.SingleOrDefault(s => s.Id == id);
            return entity != null;
        }
    }
}