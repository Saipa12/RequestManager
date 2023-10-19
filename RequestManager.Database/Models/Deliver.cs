using RequestManager.Database.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RequestManager.Database.Models;

public class Deliver : AuditableDatabaseEntity
{
    [Required]
    public string Surname { get; set; }

    [Required]
    public string Name { get; set; }

    public string Patronymic { get; set; }
    public float Experience { get; set; }
    public string DriverLicenseCategory { get; set; }

    public List<Request> Requests { get; set; }
}