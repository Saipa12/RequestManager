using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;

namespace RequestManager.API.Handlers.RequestHandler;

public record AddRequest(RequestDto RequestDto);

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
        await _requestRepository.CreateAsync(request.RequestDto);
        // return await request;
        return new AddResponse();
    }
}