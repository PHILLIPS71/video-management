using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using Giantnodes.Service.Orchestrator.Domain.Shared.Enums;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Encodes.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
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