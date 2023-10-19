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
        var record = await DatabaseContext.Requests.FirstOrDefaultAsync(x => x.Id == entity.Id) ?? throw new Exception();
        if(record.Status==RequestStatus.New)
        {
            entity.Status = RequestStatus.New;
            return await base.UpdateAsync(entity, saveChanges);
        }
        else if(record.Status == RequestStatus.InProgress)
        {
           
        }
        if (entity.Deliver is not null)
        {
            entity.Status = RequestStatus.InProgress;
            entity.Deliver = await DatabaseContext.Delivers.FirstOrDefaultAsync(x => x.Id == entity.Deliver.Id);// TODO автоматизировать
        }
        if(record.Status == RequestStatus.InProgress)
        {
            entity.Deliver = await DatabaseContext.Delivers.FirstOrDefaultAsync(x => x.Id == entity.Deliver.Id);// TODO автоматизировать
        }

        return await base.UpdateAsync(entity, saveChanges);
    }
    
    public async Task<Request> RejectedAsync(Request entity, string comment, bool saveChanges = false)
    {
        entity.Reason = comment;
        entity.Status = RequestStatus.Rejected;
        await UpdateAsync(entity);
        return await SaveAndDetachAsync(entity, saveChanges);
    }
}