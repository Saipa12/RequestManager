using RequestManager.Database.Contexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace RequestManager.Database.Factoryies;

public sealed class StorageDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql("");
        var operationalStoreOptions = Options.Create(new OperationalStoreOptions());
        return new DatabaseContext(optionsBuilder.Options, operationalStoreOptions);
    }
}