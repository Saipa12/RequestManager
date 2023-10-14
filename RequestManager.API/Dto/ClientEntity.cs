using RequestManager.API.Common;
using RequestManager.Database.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace RequestManager.API.Dto;

public abstract class ClientEntity : IMapFrom<DatabaseEntity>
{
    [Required]
    public long Id { get; set; }
}