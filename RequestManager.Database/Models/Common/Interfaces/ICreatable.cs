namespace RequestManager.Database.Models.Common.Interfaces;

public interface ICreatable
{
    public User CreatedBy { get; set; }

    public string CreatedById { get; set; }

    public DateTime? CreatedAt { get; set; }
}