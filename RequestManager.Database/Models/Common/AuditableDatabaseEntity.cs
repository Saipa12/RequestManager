using RequestManager.Database.Models.Common.Interfaces;

namespace RequestManager.Database.Models.Common;

public abstract class AuditableDatabaseEntity : DatabaseEntity, ICreatable, IUpdatable, IDeletable
{
    public User CreatedBy { get; set; }
    public string CreatedById { get; set; }
    public DateTime? CreatedAt { get; set; }

    public User UpdatedBy { get; set; }
    public string UpdatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public User DeletedBy { get; set; }
    public string DeletedById { get; set; }
    public DateTime? DeletedAt { get; set; }
}