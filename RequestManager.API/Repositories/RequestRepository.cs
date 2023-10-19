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

    public override async Task<Request> UpdateAsync(Request entity, bool saveChanges = true)
    {
        var record = await _databaseContext.Requests.FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (record is not null)
        {
            if (record.Status == RequestStatus.New)
            {
                record.Cost = 0;
                record.DeliveryDate = entity.DeliveryDate;
                record.CargoDescription = entity.CargoDescription;
                record.DeliveryTime = entity.DeliveryTime;
                record.RecipientFIO = entity.RecipientFIO;
                record.DeliveryAddress = entity.DeliveryAddress;
                record.DispatchAddress = entity.DispatchAddress;
                record.Deliver = entity.Deliver;
            }
            else if (record.Status == RequestStatus.InProgress && (entity.Status == RequestStatus.Rejected || entity.Status == RequestStatus.Completed))
            {
                record.Status = entity.Status;
            }
            if (record.Deliver is not null)
            {
                record.Status = RequestStatus.InProgress;
            }

            // Пометьте сущность как измененную
            _databaseContext.Entry(record).State = EntityState.Modified;

            if (saveChanges)
            {
                await _databaseContext.SaveChangesAsync();
            }
        }
        return await SaveAndDetachAsync(entity, saveChanges);
    }

    public async Task<Request> RejectedAsync(Request entity, string comment, bool saveChanges = false)
    {
        entity.com
        return await SaveAndDetachAsync(entity, saveChanges);
    }
}