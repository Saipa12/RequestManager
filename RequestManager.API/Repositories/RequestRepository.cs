using AutoMapper;
using RequestManager.Core.Repositories;
using RequestManager.Database.Contexts;
using RequestManager.Database.Enums;
using RequestManager.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace RequestManager.API.Repositories;

public class RequestRepository : Repository<User>
{
    public RequestRepository(DatabaseContext databaseContext, IMapper mapper) : base(databaseContext, mapper)
    {
    }

    public async Task<IEnumerable<Request>> GetRequestsAsync()
    {
        return await _databaseContext.Requests.ToListAsync();
    }

    public async Task<Request> GetRequest(int id)
    {
        return await _databaseContext.Requests.FirstAsync(x => x.Id == id);
    }

    public async Task AddRequestAsync(Request request)
    {
        await _databaseContext.Requests.AddAsync(request);
    }

    public async Task<Request> EditRequestAsync(Request request)
    {
        var record = await _databaseContext.Requests.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (record is not null)
        {
            if (record.Status == RequestStatus.New)
            {
                record.Cost = 0;
                record.DeliveryDate = request.DeliveryDate;
                record.CargoDescription = request.CargoDescription;
                record.DeliveryTime = request.DeliveryTime;
                record.RecipientFIO = request.RecipientFIO;
                record.DeliveryAddress = request.DeliveryAddress;
                record.DispatchAddress = request.DispatchAddress;
                record.Deliver = request.Deliver;
            }
            else if (record.Status == RequestStatus.InProgress && (request.Status == RequestStatus.Cancelled || request.Status == RequestStatus.Completed))
            {
                record.Status = request.Status;
            }
            if (record.Deliver is not null)
            {
                record.Status = RequestStatus.InProgress;
            }
        }
        return record;
    }

    public async Task DeleteRequestAsync(int id)
    {
        var deletedRequest = await _databaseContext.Requests.FirstAsync(x => x.Id == id);
        _databaseContext.Requests.Remove(deletedRequest);
    }
}