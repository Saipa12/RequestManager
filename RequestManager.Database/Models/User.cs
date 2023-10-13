using RequestManager.Database.Models.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestManager.Database.Models;

public class User : IdentityUser, ICreatable, IUpdatable, IDeletable
{
    [NotMapped] public User CreatedBy { get; set; }
    [NotMapped] public string CreatedById { get; set; }
    public DateTime? CreatedAt { get; set; }

    [NotMapped] public User UpdatedBy { get; set; }
    [NotMapped] public string UpdatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [NotMapped] public User DeletedBy { get; set; }
    [NotMapped] public string DeletedById { get; set; }
    public DateTime? DeletedAt { get; set; }
}