namespace RequestManager.Database.Models.Common.Interfaces;

public interface IDeletable
{
    public User DeletedBy { get; set; }

    public string DeletedById { get; set; }

    public DateTime? DeletedAt { get; set; }
}