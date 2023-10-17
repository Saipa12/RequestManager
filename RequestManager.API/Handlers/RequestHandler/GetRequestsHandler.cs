using AutoMapper;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using Microsoft.EntityFrameworkCore;

namespace RequestManager.API.Handlers.RequestHandler;

public record GetRequests(bool IncludeDeliver = false);

public record GetResponses(IEnumerable<RequestDto> RequestDto);

public class GetRequestsHandler : IAsyncHandler<GetRequests, GetResponses>
{
    private readonly RequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public GetRequestsHandler(RequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<GetResponses> Handle(GetRequests request)
    {
        if (request.IncludeDeliver)
        {
            var requests = await _requestRepository.GetAsync(x => x.Include(d => d.Deliver));
            var response = requests.Select(_mapper.Map<RequestDto>);
            return new GetResponses(response);
        }
        else
        {
            var requests = await _requestRepository.GetAsync(); // Дожидаемся выполнения задачи
            var response = requests.Select(_mapper.Map<RequestDto>);
            return new GetResponses(response);
        }
    }
}