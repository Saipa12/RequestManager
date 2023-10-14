using AutoMapper;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Database.Models;

namespace RequestManager.API.Handlers.RequestHandler;

public record EditRequest(RequestDto Request);

public record EditResponse(RequestDto Request);

public class EditRequestHandler : IAsyncHandler<EditRequest, EditResponse>
{
    private readonly RequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public EditRequestHandler(RequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<EditResponse> Handle(EditRequest request)
    {
        var updatedRequest = _mapper.Map<Request>(request.Request);
        var updatedRequestDto = _mapper.Map<RequestDto>(await _requestRepository.UpdateAsync(updatedRequest));
        return new EditResponse(updatedRequestDto);
    }
}