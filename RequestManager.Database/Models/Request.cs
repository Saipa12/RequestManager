using RequestManager.Database.Enums;
using RequestManager.Database.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace RequestManager.Database.Models;

public class Request : AuditableDatabaseEntity
{
    [Required]
    public RequestStatus Status { get; set; }

    public string CargoDescription { get; set; }

    [Required]
    public string DeliveryAddress { get; set; }

    [Required]
    public string DispatchAddress { get; set; }

    [Required]
    public DateTime DeliveryDate { get; set; }

    [Required]
    public DateTime DeliveryTime { get; set; }

    public float Cost { get; set; }
    public string TelNumber { get; set; }
    public string RecipientFIO { get; set; }
    public string Reason { get; set; }
    public Deliver Deliver { get; set; }
}