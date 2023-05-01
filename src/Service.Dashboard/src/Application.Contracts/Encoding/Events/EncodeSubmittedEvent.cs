namespace Giantnodes.Service.Dashboard.Application.Contracts.Encoding.Events;

public sealed record EncodeSubmittedEvent
{
    public required Guid Id { get; init; }

    public required Guid PresetId { get; init; }
    
    public required string FullPath { get; init; }

    public required DateTime Timestamp { get; init; }
}