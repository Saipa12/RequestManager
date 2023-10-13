using System.ComponentModel.DataAnnotations;

namespace RequestManager.API.Dto;

public abstract class ClientEntity
{
    [Required]
    public long Id { get; set; }
}