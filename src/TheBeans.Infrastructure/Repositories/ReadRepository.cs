using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheBeans.Core.Common;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Infrastructure.Data;

namespace TheBeans.Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository for read-only operations on entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadRepository{T}"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ReadRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Checks if any entities satisfy the given condition asynchronously.
        /// </summary>
        /// <param name="expression">The condition to evaluate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if any entities satisfy the condition; otherwise, false.</returns>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
             
            return await _context.Set<T>().AnyAsync(expression);
        }

        /// <summary>
        /// Retrieves all entities without tracking.
        /// </summary>
        /// <returns>An <see cref="IQueryable{T}"/> that can be used to enumerate all entities.</returns>
        public IQueryable<T> GetAll() => _context.Set<T>().AsNoTracking();

        /// <summary>
        /// Retrieves all entities with optional tracking.
        /// </summary>
        /// <param name="trackChanges">If set to true, changes to the entities will be tracked; otherwise, they will not be tracked.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that can be used to enumerate all entities.</returns>
        public IQueryable<T> GetAll(bool trackChanges = false)
        {
             
            return trackChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// Retrieves all entities asynchronously with optional tracking.
        /// </summary>
        /// <param name="trackChanges">If set to true, changes to the entities will be tracked; otherwise, they will not be tracked.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all entities.</returns>
        public async Task<List<T>> GetAllAsync(bool trackChanges = false)
        {
            return await (trackChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking()).ToListAsync();
        }

        /// <summary>
        /// Retrieves entities that satisfy the given condition with optional tracking.
        /// </summary>
        /// <param name="expression">The condition to evaluate.</param>
        /// <param name="trackChanges">If set to true, changes to the entities will be tracked; otherwise, they will not be tracked.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that can be used to enumerate the entities that satisfy the condition.</returns>
        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
        {
            return trackChanges ? _context.Set<T>().Where(expression) : _context.Set<T>().Where(expression).AsNoTracking();
        }

        /// <summary>
        /// Retrieves an entity by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Includes related entities in the query result.
        /// </summary>
        /// <param name="includes">The related entities to include.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that includes the specified related entities.</returns>
        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }
    }
}
