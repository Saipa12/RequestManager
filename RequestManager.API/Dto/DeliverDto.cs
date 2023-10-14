using RequestManager.API.Common;
using RequestManager.Database.Models;

namespace RequestManager.API.Dto;

public class DeliverDto : ClientEntity, IMapFrom<Deliver>
{
    public string Name { get; set; }

    public List<RequestDto> Requests { get; set; }
}