using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;

public sealed record FileEncodeSpeedChangedEvent : IDomainEvent
{
    public required Guid FileId { get; init; }

    public required Guid EncodeId { get; init; }

    public required float Frames { get; set; }

    public required long Bitrate { get; set; }

    public required float Scale { get; set; }

    public required DateTime RaisedAt { get; init; }
}