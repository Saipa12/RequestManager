using AutoMapper;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Database.Models;

namespace RequestManager.API.Handlers.RequestHandler;

public record AddRequest(RequestDto RequestDto);

public record AddResponse();

public class AddRequestHandler : IAsyncHandler<AddRequest, AddResponse>
{
    private readonly RequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public AddRequestHandler(RequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<AddResponse> Handle(AddRequest request)
    {
        var addedRequest = _mapper.Map<Request>(request.RequestDto);
        await _requestRepository.CreateAsync(addedRequest);
        return new AddResponse();
    }
}