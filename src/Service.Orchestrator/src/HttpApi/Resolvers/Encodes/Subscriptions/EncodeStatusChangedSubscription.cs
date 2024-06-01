using Giantnodes.Service.Orchestrator.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Encodes.Subscriptions;

[ExtendObjectType(OperationTypeNames.Subscription)]
public class EncodeStatusChangedSubscription
{
    [Subscribe]
    [Topic(nameof(EncodeStatusChangedEvent))]
    [UseSingleOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Encode> EncodeStatusChanged(
        [Service] ApplicationDbContext database,
        [EventMessage] Encode encode)
    {
        return database.Encodes.Where(x => x.Id == encode.Id).AsNoTracking();
    }
}