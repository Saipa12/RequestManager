using RequestManager.Core.Services;
using RequestManager.Database.Contexts;

namespace RequestManager.API.Services;

public class ImageService : IService
{
    private readonly DatabaseContext _databaseContext;

    public ImageService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
}