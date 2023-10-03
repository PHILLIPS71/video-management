namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;

public sealed record LibraryFileCreatedEvent
{
    public required Guid Id { get; init; }

    public required string FullPath { get; init; }
}