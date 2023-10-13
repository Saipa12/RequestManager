using Microsoft.EntityFrameworkCore;

namespace RequestManager.Database.Extensions;

public static class DbContextExtensions
{
    public static void DetachRange<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities) where TEntity : class
    {
        foreach (var entity in entities)
        {
            dbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}