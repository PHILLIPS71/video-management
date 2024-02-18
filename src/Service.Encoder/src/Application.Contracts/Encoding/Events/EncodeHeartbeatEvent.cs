using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeHeartbeatEvent : IntegrationEvent
{
    public required float Frames { get; set; }

    public required long Bitrate { get; set; }

    public required float Scale { get; set; }
}