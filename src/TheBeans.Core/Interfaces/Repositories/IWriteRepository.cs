namespace TheBeans.Core.Interfaces.Repositories
{
    public interface IWriteRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteByIdAsync(int id);
    Task SaveChangesAsync();
}
}

