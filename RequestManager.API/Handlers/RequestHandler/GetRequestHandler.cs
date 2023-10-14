using AutoMapper;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Database.Models;

namespace RequestManager.API.Handlers.RequestHandler;

public record GetRequest(int Id);

public record GetResponse(RequestDto Request);

public class GetRequestHandler : IAsyncHandler<GetRequest, GetResponse>
{
    private readonly RequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public GetRequestHandler(RequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<GetResponse> Handle(GetRequest request)
    {
        var requestDto = new RequestDto() { Id = request.Id };
        var getedRequestDto = _mapper.Map<Request>(requestDto);
        var gettedRequest = _mapper.Map<RequestDto>(await _requestRepository.GetFirstOrDefaultAsync(x => x.Id == getedRequestDto.Id));
        return new GetResponse(gettedRequest);
    }
}