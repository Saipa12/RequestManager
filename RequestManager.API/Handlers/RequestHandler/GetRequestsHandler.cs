using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;

namespace RequestManager.API.Handlers.RequestHandler;

public record GetRequests(bool IncludeDeliver = false, int PageNumber = 1, int PageSize = 10);

public record GetResponses(IEnumerable<RequestDto> RequestDto, int Count);

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
        var skip = request.PageNumber * request.PageSize;
        var count = await _requestRepository.GetCount();
        var query = await _requestRepository.GetAsync(x =>
        {
            x = x.Skip(skip).Take(request.PageSize);
            if (request.IncludeDeliver)
            {
                x = x.Include(d => d.Deliver);
            }
            return x;
        });

        var requests = query.ToList();
        var response = requests.Select(_mapper.Map<RequestDto>);
        return new GetResponses(response, count);
    }
}