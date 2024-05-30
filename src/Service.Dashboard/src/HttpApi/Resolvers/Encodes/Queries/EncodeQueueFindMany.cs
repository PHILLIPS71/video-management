using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Encodes.Queries;

[QueryType]
public class EncodeQueueFindMany
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    public IQueryable<Encode> EncodeQueue([Service] ApplicationDbContext database)
    {
        return database
            .Encodes
            .AsNoTracking()
            .OrderByDescending(x => x.Status == EncodeStatus.Encoding)
            .ThenByDescending(x => x.Status == EncodeStatus.Queued)
            .ThenByDescending(x => x.Status == EncodeStatus.Submitted)
            .ThenByDescending(x => x.CompletedAt ?? x.CancelledAt ?? x.FailedAt ?? x.CreatedAt);
    }
}