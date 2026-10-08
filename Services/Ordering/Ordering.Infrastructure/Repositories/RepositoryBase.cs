using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories
{
    public class RepositoryBase<T>(OrderContext orderContext) : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly OrderContext orderContext = orderContext;

        public async Task<T> AddAsync(T entity)
        {
            orderContext.Set<T>().Add(entity);
            await orderContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            orderContext.Set<T>().Remove(entity);
            await orderContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await orderContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await orderContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await orderContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            orderContext.Entry(entity).State = EntityState.Modified;
            orderContext.Set<T>().Update(entity);
            await orderContext.SaveChangesAsync();
        }
    }
}
