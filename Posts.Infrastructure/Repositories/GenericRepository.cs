using PostsProject.Infrastructure.Context;
using PostsProject.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PostsProject.Infrastructure.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {
        public virtual IQueryable<T> GetAllNoTracking() => context.Set<T>().AsNoTracking().AsQueryable();
        public virtual IQueryable<T> GetAllWithTracking() => context.Set<T>().AsQueryable();
        public virtual async Task<bool> IsExistAsync(Expression<Func<T, bool>> match) => await context.Set<T>().AnyAsync(match);
        public virtual async Task<T> GetByIdAsync(int id) => (await context.Set<T>().FindAsync(id))!;
        public virtual async Task<bool> AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            return await context.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            return await context.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            return await context.SaveChangesAsync() > 0;
        }
    }
}