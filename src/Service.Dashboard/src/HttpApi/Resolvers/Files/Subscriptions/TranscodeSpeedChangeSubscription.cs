using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Files.Subscriptions;

[ExtendObjectType(OperationTypeNames.Subscription)]
public class TranscodeSpeedChangeSubscription
{
    [Subscribe]
    [Topic(nameof(FileTranscodeSpeedChangedEvent))]
    [UseSingleOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Transcode> TranscodeSpeedChange(
        [Service] ApplicationDbContext database,
        [EventMessage] Transcode transcode)
    {
        return database.Transcodes.Where(x => x.Id == transcode.Id).AsNoTracking();
    }
}