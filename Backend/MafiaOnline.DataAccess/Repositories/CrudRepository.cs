using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface ICrudRepository<T> where T : Entity
    {

        public Task<IList<T>> GetAllAsync();
        public Task<T> GetByIdAsync(long id);
        public Task<IList<T>> GetByIdsAsync(long[] ids);
        public Task<T> GetRandomAsync();
        public Task<IList<T>> GetRandomsAsync(int count);
        public long Create(T model);
        public void Update(T model);
        public void UpdateRange(T[] models);
        public void DeleteById(long id);
        public void DeleteByIds(long[] ids);
        public bool Exists(long id);
        public Task<IList<T>> FindAsync(T filterObject);

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

        public async virtual Task<IList<T>> GetRandomsAsync(int count)
        {
            return await entities.OrderBy(r => Guid.NewGuid()).Take(count).ToListAsync();
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

        public virtual void UpdateRange(T[] models)
        {
            entities.UpdateRange(models);
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

        public async virtual Task<IList<T>> FindAsync(T filterObject)
        {
            if (filterObject == null)
            {
                throw new ArgumentNullException(nameof(filterObject));
            }

            // Get properties of object T
            var properties = typeof(T).GetProperties();

            // Create expression parameter
            var parameter = Expression.Parameter(typeof(T), "x");

            // Create a list of conditions for the expression
            var conditions = new List<Expression>();

            // Create conditions for each property if it's not null and not a default value (for value types)
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(filterObject);
                if (propertyValue != null && (property.PropertyType.IsClass || !propertyValue.Equals(Activator.CreateInstance(propertyValue.GetType()))))
                {
                    var condition = Expression.Equal(
                        Expression.Property(parameter, property),
                        Expression.Constant(propertyValue)
                    );

                    conditions.Add(condition);
                }
            }

            // Combine conditions using "And" logical expression
            Expression conditionExpression = conditions.Any()
                ? conditions.Aggregate(Expression.And)
                : Expression.Constant(true);

            // Create lambda expression based on the conditions
            var lambdaExpression = Expression.Lambda<Func<T, bool>>(conditionExpression, parameter);

            // Execute the query in the database
            return await _context.Set<T>().Where(lambdaExpression).ToListAsync();
        }
    }
}