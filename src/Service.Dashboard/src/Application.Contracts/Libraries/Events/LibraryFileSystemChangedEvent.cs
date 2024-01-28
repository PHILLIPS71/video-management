using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;

public record LibraryFileSystemChangedEvent : IIntegrationEvent
{
    public required Guid LibraryId { get; init; }

    public required WatcherChangeTypes ChangeTypes { get; init; }

    public required string FullPath { get; init; }

    public required DateTime RaisedAt { get; init; }
}