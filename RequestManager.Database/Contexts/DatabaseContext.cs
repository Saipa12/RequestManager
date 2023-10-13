using RequestManager.Database.Models;
using RequestManager.Database.Models.Common.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace RequestManager.Database.Contexts;

public class DatabaseContext : ApiAuthorizationDbContext<User>
{
    public DbSet<Deliver> Delivers { get; set; }
    public DbSet<Request> Requests { get; set; }

    public DatabaseContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IDeletable>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    //entry.Property(nameof(IDeletable.DeletedBy)).CurrentValue = _currentUser.UserId; // TODO
                    entry.Property(nameof(IDeletable.DeletedAt)).CurrentValue = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasOne(x => x.CreatedBy).WithOne().HasForeignKey<User>(x => x.CreatedById);
        builder.Entity<User>().HasOne(x => x.UpdatedBy).WithOne().HasForeignKey<User>(x => x.UpdatedById);
        builder.Entity<User>().HasOne(x => x.DeletedBy).WithOne().HasForeignKey<User>(x => x.DeletedById);
        ConfigureSoftDeleteFilter(builder);

        base.OnModelCreating(builder);
    }

    private static void ConfigureSoftDeleteFilter(ModelBuilder builder)
    {
        foreach (var softDeletableTypeBuilder in builder.Model.GetEntityTypes().Where(x => typeof(IDeletable).IsAssignableFrom(x.ClrType)))
        {
            var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");

            softDeletableTypeBuilder.SetQueryFilter(
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, nameof(IDeletable.DeletedAt)),
                        Expression.Constant(null)),
                    parameter)
            );
        }
    }
}