using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;

public sealed record EncodeSpeedChangedEvent : IDomainEvent
{
    public required Guid EncodeId { get; init; }

    public required float Frames { get; set; }

    public required long Bitrate { get; set; }

    public required float Scale { get; set; }

    public required DateTime RaisedAt { get; init; }
}