using AutoMapper;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Database.Models;
using RequestManager.Shared.Services;
using System;

namespace RequestManager.API.Handlers.RequestHandler;

public record RejectedRequest(RequestDto Request);

public record RejectedResponse();

public class RejectedRequestHandler : IAsyncHandler<RejectedRequest, RejectedResponse>
{
    private readonly RequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public RejectedRequestHandler(RequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<RejectedResponse> Handle(RejectedRequest request)
    {
        var deletedRequest = _mapper.Map<Request>(request.Request);
        await _requestRepository.DeleteAsync(deletedRequest);
        return new RejectedResponse();
    }
}