using RequestManager.API.Services;
using RequestManager.Core.Handlers;

namespace RequestManager.API.Handlers;

public record GetImagesRequest;
public record GetImagesResponse;

public class GetImagesHandler : IAsyncHandler<GetImagesRequest, GetImagesResponse>
{
    private readonly ImageService _imageService;

    public GetImagesHandler(ImageService imageService)
    {
        _imageService = imageService;
    }

    public Task<GetImagesResponse> Handle(GetImagesRequest request)
    {
        return Task.FromResult(new GetImagesResponse());
    }
}