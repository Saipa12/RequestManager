﻿using RequestManager.Api.Enums;

namespace RequestManager.API.Dto;

public class RequestDto : ClientEntity
{
    public RequestStatus Status { get; set; }

    public string Description { get; set; }

    public string DeliveryAddress { get; set; }

    public string DispatchAddress { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime DeliveryTime { get; set; }

    public float Cost { get; set; }
    public string TelNumber { get; set; }
    public string RecipientFIO { get; set; }

    public DeliverDto Deliver { get; set; }
}