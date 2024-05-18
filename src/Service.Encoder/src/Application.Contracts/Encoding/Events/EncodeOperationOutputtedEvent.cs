using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeOperationOutputtedEvent : IntegrationEvent
{
    public readonly struct ConversionSpeed
    {
        public required float Frames { get; init; }

        public required long Bitrate { get; init; }

        public required float Scale { get; init; }
    }

    public required Guid JobId { get; init; }

    public required string Output { get; init; }

    public required ConversionSpeed? Speed { get; init; }
}