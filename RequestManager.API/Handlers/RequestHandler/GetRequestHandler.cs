﻿using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Shared.Services;
using DbM = RequestManager.Database.Models;

namespace RequestManager.API.Handlers.RequestHandler;

public record GetRequest(int Id);

public record GetResponses(DbM.Request RequestDto);

public class GetRequestHandler : IAsyncHandler<GetRequest, GetResponses>
{
    private readonly RequestRepository _requestRepository;

    public GetRequestHandler(RequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<GetResponses> Handle(GetRequest request)
    {
        return new GetResponses(await _requestRepository.GetRequest(request.Id));
        throw new NotImplementedException();
    }
}