using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Encodes.Events;

public sealed record EncodeSpeedChangedEvent : DomainEvent
{
    public required Guid EncodeId { get; init; }

    public required float Frames { get; set; }

    public required long Bitrate { get; set; }

    public required float Scale { get; set; }
}