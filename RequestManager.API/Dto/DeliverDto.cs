namespace RequestManager.API.Dto;

public class DeliverDto : ClientEntity
{
    public string Name { get; set; }

    public List<RequestDto> Requests { get; set; }
}