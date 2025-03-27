using System.Linq.Expressions;

namespace PostsProject.Infrastructure.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllNoTracking();
        IQueryable<T> GetAllWithTracking();
        Task<bool> IsExistAsync(Expression<Func<T, bool>> match);
        Task<T> GetByIdAsync(int id);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

    }
}
