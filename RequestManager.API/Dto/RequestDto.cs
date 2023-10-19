using RequestManager.Api.Enums;
using RequestManager.API.Common;
using RequestManager.Database.Models;

namespace RequestManager.API.Dto;

public class RequestDto : ClientEntity, IMapFrom<Request>
{
    public RequestStatus Status { get; set; }

    public string CargoDescription { get; set; }

    public string DeliveryAddress { get; set; }

    public string DispatchAddress { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime DeliveryTime { get; set; }

    public float Cost { get; set; }
    public string TelNumber { get; set; }
    public string RecipientFIO { get; set; }
    public string Comment { get; init; }

    public DeliverDto Deliver { get; set; }
}