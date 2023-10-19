using AutoMapper;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Database.Models;
using RequestManager.Shared.Services;
using System;

namespace RequestManager.API.Handlers.RequestHandler;

public record RejectedRequest(RequestDto Request, string Reason);

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
        var rejectedRequest = _mapper.Map<Request>(request.Request);
        await _requestRepository.RejectedAsync(rejectedRequest, request.Reason);
        return new RejectedResponse();
    }
}