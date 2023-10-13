using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Shared.Services;
using DbM = RequestManager.Database.Models;

namespace RequestManager.API.Handlers.RequestHandler;

public record EditRequest(DbM.Request RequestDto);

public record EditResponse(DbM.Request RequestDto);

public class EditRequestHandler : IAsyncHandler<EditRequest, EditResponse>
{
    private readonly RequestRepository _requestRepository;

    public EditRequestHandler(RequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<EditResponse> Handle(EditRequest request)
    {
        return new EditResponse(await _requestRepository.EditRequestAsync(request.RequestDto));
    }
}