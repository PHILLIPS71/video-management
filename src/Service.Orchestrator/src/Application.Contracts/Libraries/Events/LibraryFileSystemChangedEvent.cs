using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Libraries.Events;

public sealed record LibraryFileSystemChangedEvent : IntegrationEvent
{
    public required Guid LibraryId { get; init; }

    public required WatcherChangeTypes ChangeTypes { get; init; }

    public required string FilePath { get; init; }
}