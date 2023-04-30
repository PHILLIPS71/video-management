namespace Giantnodes.Service.Dashboard.Application.Contracts.Probing.Events;

public sealed record ProbeSubmittedEvent
{
    public required Guid Id { get; init; }

    public required string FullPath { get; init; }

    public required DateTime Timestamp { get; init; }
}