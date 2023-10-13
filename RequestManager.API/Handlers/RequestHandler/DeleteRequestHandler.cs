using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Shared.Services;

namespace RequestManager.API.Handlers.RequestHandler;

public record DeleteRequest(RequestDto Request);

public record DeleteResponse();

public class DeleteRequestHandler : IAsyncHandler<DeleteRequest, DeleteResponse>
{
    private readonly RequestRepository _requestRepository;

    public DeleteRequestHandler(RequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<DeleteResponse> Handle(DeleteRequest request)
    {
        await _requestRepository.DeleteAsync(request.Request);
        return new DeleteResponse();
    }
}