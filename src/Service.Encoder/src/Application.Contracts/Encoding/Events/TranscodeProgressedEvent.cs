namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record TranscodeProgressedEvent
{
    public required Guid JobId { get; init; }
    
    public required string FullPath { get; init; }

    public required TimeSpan Duration { get; init; }

    public required TimeSpan TotalLength { get; init; }

    public required float Percent { get; init; }
}