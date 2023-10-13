using AutoMapper;
using RequestManager.Core.Repositories;
using RequestManager.Database.Contexts;
using RequestManager.Database.Enums;
using RequestManager.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace RequestManager.API.Repositories;

public class RequestRepository : Repository<Request>
{
    public RequestRepository(DatabaseContext databaseContext, IMapper mapper) : base(databaseContext, mapper)
    {
    }
}