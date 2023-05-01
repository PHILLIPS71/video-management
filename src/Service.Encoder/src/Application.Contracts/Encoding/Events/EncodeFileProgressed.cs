namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeFileProgressed
{
    public required Guid JobId { get; init; }

    public required TimeSpan Duration { get; init; }

    public required TimeSpan TotalLength { get; init; }

    public required int Percent { get; init; }
}