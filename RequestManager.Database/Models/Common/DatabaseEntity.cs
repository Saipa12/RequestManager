using RequestManager.Database.Models.Common.Interfaces;

namespace RequestManager.Database.Models.Common;

public abstract class DatabaseEntity : IDatabaseEntity
{
    public long Id { get; set; }
}