using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;

namespace RequestManager.API.Handlers.RequestHandler;

public record EditRequest(RequestDto Request);

public record EditResponse(RequestDto Request);

public class EditRequestHandler : IAsyncHandler<EditRequest, EditResponse>
{
    private readonly RequestRepository _requestRepository;

    public EditRequestHandler(RequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<EditResponse> Handle(EditRequest request)
    {
        return new EditResponse(await _requestRepository.UpdateAsync(request.RequestDto));
    }
}