using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheBeans.Core.Common;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Infrastructure.Data;

namespace TheBeans.Infrastructure.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;

    public ReadRepository(AppDbContext context)
    {
        _context = context;
    }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll() => _context.Set<T>().AsNoTracking();

        public IQueryable<T> GetAll(bool trackChanges = false)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetByIdAsync(Guid id) => 
        await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        public Task<T?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }
    }
}