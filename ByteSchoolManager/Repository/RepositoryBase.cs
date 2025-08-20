using System.Linq.Expressions;
using ByteSchoolManager.Common.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Repository;

public abstract class RepositoryBase<T> where T : class, IDbEntity
{
    protected RepositoryBase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected readonly ApplicationDbContext _dbContext;

    public abstract IQueryable<T> Query { get; }

    public async Task<TResult?> FirstOrDefaultSelectionAsync<TResult>(Expression<Func<T, TResult>> select,
        Expression<Func<T, bool>>? predicate = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Query : Query.AsNoTracking();

        query = predicate is not null ? query.Where(predicate) : query;

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        
        return await query.Select(select).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
        List<Expression<Func<T, object>>>? includes = null,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Query : Query.AsNoTracking();

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        
        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Query : Query.AsNoTracking();

        query = predicate is not null ? query.Where(predicate) : query;

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<List<TResult>> ListSelectionAsync<TResult>(Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? predicate = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Query : Query.AsNoTracking();

        query = predicate is not null ? query.Where(predicate) : query;

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.Select(selector).ToListAsync();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(entity, cancellationToken);
    }

    public void Remove(T entity)
    {
        _dbContext.Remove(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddRangeAsync(entities, cancellationToken);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbContext.RemoveRange(entities);
    }
}