namespace RequestManager.Database.Models.Common.Interfaces;

public interface IUpdatable
{
    public User UpdatedBy { get; set; }

    public string UpdatedById { get; set; }

    public DateTime? UpdatedAt { get; set; }
}