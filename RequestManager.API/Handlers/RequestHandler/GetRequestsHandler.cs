using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Shared.Services;

using DbM = RequestManager.Database.Models;

namespace RequestManager.API.Handlers.RequestHandler;

public record GetRequests(int Id);

public record GetResponse(IEnumerable<DbM.Request> RequestDto);

public class GetRequestsHandler : IAsyncHandler<GetRequests, GetResponse>
{
    private readonly RequestRepository _requestRepository;

    public GetRequestsHandler(RequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<GetResponse> Handle(GetRequests request)
    {
        return new GetResponse(await _requestRepository.GetRequestsAsync());
    }
}