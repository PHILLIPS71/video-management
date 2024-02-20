using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeOperationEncodeHeartbeatEvent : IntegrationEvent
{
    public required Guid JobId { get; init; }

    public required float Frames { get; init; }

    public required long Bitrate { get; init; }

    public required float Scale { get; init; }
}