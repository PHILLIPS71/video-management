using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;

public class LibraryMonitoringChangedEvent : IDomainEvent
{
    public required Guid LibraryId { get; init; }

    public required bool IsMonitoring { get; init; }

    public required DateTime RaisedAt { get; init; }
}