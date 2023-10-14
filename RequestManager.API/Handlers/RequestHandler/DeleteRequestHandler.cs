using AutoMapper;
using RequestManager.API.Dto;
using RequestManager.API.Repositories;
using RequestManager.Core.Handlers;
using RequestManager.Database.Models;
using RequestManager.Shared.Services;
using System;

namespace RequestManager.API.Handlers.RequestHandler;

public record DeleteRequest(RequestDto Request);

public record DeleteResponse();

public class DeleteRequestHandler : IAsyncHandler<DeleteRequest, DeleteResponse>
{
    private readonly RequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public DeleteRequestHandler(RequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<DeleteResponse> Handle(DeleteRequest request)
    {
        var deletedRequest = _mapper.Map<Request>(request.Request);
        await _requestRepository.DeleteAsync(deletedRequest);
        return new DeleteResponse();
    }
}