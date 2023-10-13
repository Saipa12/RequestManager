using AutoMapper;
using RequestManager.Database.Contexts;
using RequestManager.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RequestManager.Core.Repositories;

public abstract class Repository<TEntity> : IRepository where TEntity : class
{
    protected readonly DatabaseContext _databaseContext;
    protected readonly IMapper _mapper;

    public Repository(DatabaseContext databaseContext, IMapper mapper)
    {
        _databaseContext = databaseContext;
        _mapper = mapper;
    }

    public virtual async Task<TEntity> GetFirstOrDefaultAsync() => await _databaseContext.Set<TEntity>().FirstOrDefaultAsync();

    public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) => await _databaseContext.Set<TEntity>().FirstOrDefaultAsync(predicate);

    public virtual async Task<IEnumerable<TEntity>> GetAsync() => await _databaseContext.Set<TEntity>().ToListAsync();

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate) => await _databaseContext.Set<TEntity>().Where(predicate).ToListAsync();

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _databaseContext.Set<TEntity>().AddAsync(entity);
        return await SaveAndDetachAsync(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities)
    {
        await _databaseContext.Set<TEntity>().AddRangeAsync(entities);
        return await SaveAndDetachAsync(entities);
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _databaseContext.Entry(entity).State = EntityState.Modified;
        return await SaveAndDetachAsync(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities)
    {
        _databaseContext.AttachRange(entities);
        return await SaveAndDetachAsync(entities);
    }

    public virtual async Task<TEntity> DeleteAsync(TEntity entity)
    {
        _databaseContext.Entry(entity).State = EntityState.Deleted;
        return await SaveAndDetachAsync(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities)
    {
        _databaseContext.RemoveRange(entities);
        return await SaveAndDetachAsync(entities);
    }

    protected async Task<TEntity> SaveAndDetachAsync(TEntity entity)
    {
        var a = await _databaseContext.SaveChangesAsync();
        _databaseContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    protected async Task<IEnumerable<TEntity>> SaveAndDetachAsync(IEnumerable<TEntity> entities)
    {
        await _databaseContext.SaveChangesAsync();
        _databaseContext.DetachRange(entities);
        return entities;
    }
}