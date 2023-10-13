using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using DbM = RequestManager.Database.Models;

namespace RequestManager.API.Handlers.RequestHandler;

public record AddRequest(DbM.Request RequestDto);

public record AddResponse();

public class AddRequestHandler : IAsyncHandler<AddRequest, AddResponse>
{
    private readonly RequestRepository _requestRepository;

    public AddRequestHandler(RequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<AddResponse> Handle(AddRequest request)
    {
        await _requestRepository.AddRequestAsync(request.RequestDto);
        // return await request;
        return new AddResponse();
    }
}