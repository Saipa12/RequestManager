using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;

namespace RequestManager.API.Handlers.DeliverHandler;

public record GetDeliverRequests(bool WhithRequsts = false);

public record GetDeliverResponses(IEnumerable<DeliverDto> DeliverDto);

public class GetDeliverHandler : IAsyncHandler<GetDeliverRequests, GetDeliverResponses>
{
    private readonly DeliverRepository _deliverRepository;
    private readonly IMapper _mapper;

    public GetDeliverHandler(DeliverRepository deliverRepository, IMapper mapper)
    {
        _deliverRepository = deliverRepository;
        _mapper = mapper;
    }

    public async Task<GetDeliverResponses> Handle(GetDeliverRequests request)
    {
        if (request.WhithRequsts)
        {
            var requests = await _deliverRepository.GetAsync(x => x.Include(r => r.Requests));
            var response = requests.Select(_mapper.Map<DeliverDto>);
            return new GetDeliverResponses(response);
        }
        else
        {
            var requests = await _deliverRepository.GetAsync(); // Дожидаемся выполнения задачи
            var response = requests.Select(_mapper.Map<DeliverDto>);
            return new GetDeliverResponses(response);
        }
    }
}