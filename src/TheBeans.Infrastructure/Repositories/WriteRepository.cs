using TheBeans.Core.Common;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Infrastructure.Data;

namespace TheBeans.Infrastructure.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;

    public WriteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
    
    public async Task UpdateAsync(T entity) => _context.Set<T>().Update(entity);
    
    public async Task DeleteAsync(T entity) => _context.Set<T>().Remove(entity);
    
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public Task AddRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}