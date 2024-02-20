using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;

public sealed record LibraryMonitoringChangedEvent : DomainEvent
{
    public required Guid LibraryId { get; init; }

    public required bool IsMonitoring { get; init; }
}