using System.Linq.Expressions;

namespace TheBeans.Core.Interfaces.Repositories;

public interface IReadRepository<T> where T : class
{
    IQueryable<T> GetAll(bool trackChanges = false);
    IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
    Task<T?> GetByIdAsync(int id);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
}