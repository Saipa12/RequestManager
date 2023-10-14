using AutoMapper;
using RequestManager.Database.Contexts;
using RequestManager.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RequestManager.Core.Repositories;

public abstract class Repository<TEntity> : IRepository where TEntity : class
{
    protected readonly DatabaseContext DatabaseContext;
    protected readonly IMapper Mapper;

    protected Repository(DatabaseContext databaseContext, IMapper mapper)
    {
        DatabaseContext = databaseContext;
        Mapper = mapper;
    }

    public virtual async Task<TEntity> GetFirstOrDefaultAsync() => await DatabaseContext.Set<TEntity>().FirstOrDefaultAsync();

    public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) => await DatabaseContext.Set<TEntity>().FirstOrDefaultAsync(predicate);

    public virtual async Task<TEntity> GetFirstOrDefaultAsync(Func<Task<TEntity>, Task<TEntity>> func) => await func(_databaseContext.Set<TEntity>().FirstOrDefaultAsync());

    public virtual async Task<IEnumerable<TEntity>> GetAsync() => await _databaseContext.Set<TEntity>().ToListAsync();

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate) => await DatabaseContext.Set<TEntity>().Where(predicate).ToListAsync();

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func) => await func(_databaseContext.Set<TEntity>()).ToListAsync();

    public virtual async Task<TEntity> CreateAsync(TEntity entity, bool saveChanges = true)
    {
        await DatabaseContext.Set<TEntity>().AddAsync(entity);
        return await SaveAndDetachAsync(entity, saveChanges);
    }

    public virtual async Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
    {
        var enumerable = entities.ToList();
        await DatabaseContext.Set<TEntity>().AddRangeAsync(enumerable);
        return await SaveAndDetachAsync(enumerable, saveChanges);
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool saveChanges = true)
    {
        //_databaseContext.Entry(entity).State = EntityState.Unchanged; // TODO
        DatabaseContext.Update(entity);
        return await SaveAndDetachAsync(entity, saveChanges);
    }

    public virtual async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
    {
        //_databaseContext.AttachRange(entities); // TODO
        var list = entities.ToList();
        DatabaseContext.UpdateRange(list);
        return await SaveAndDetachAsync(list, saveChanges);
    }

    public virtual async Task<TEntity> DeleteAsync(TEntity entity, bool saveChanges = true)
    {
        //_databaseContext.Entry(entity).State = EntityState.Deleted; // TODO
        DatabaseContext.Remove(entity);
        return await SaveAndDetachAsync(entity, saveChanges);
    }

    public virtual async Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
    {
        var list = entities.ToList();
        DatabaseContext.RemoveRange(list);
        return await SaveAndDetachAsync(list, saveChanges);
    }

    protected async Task<TEntity> SaveAndDetachAsync(TEntity entity, bool saveChanges = true)
    {
        await DatabaseContext.SaveChangesAsync();
        DatabaseContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    protected async Task<IEnumerable<TEntity>> SaveAndDetachAsync(List<TEntity> entities, bool saveChanges = true)
    {
        await DatabaseContext.SaveChangesAsync();
        DatabaseContext.DetachRange(entities);
        return entities;
    }
}