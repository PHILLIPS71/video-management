using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Encodes.Subscriptions;

[ExtendObjectType(OperationTypeNames.Subscription)]
public class EncodeOutputtedSubscription
{
    [Subscribe]
    [Topic(nameof(EncodeOutputtedEvent))]
    [UseSingleOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Encode> EncodeOutputted(
        [Service] ApplicationDbContext database,
        [EventMessage] Encode encode)
    {
        return database.Encodes.Where(x => x.Id == encode.Id).AsNoTracking();
    }
}