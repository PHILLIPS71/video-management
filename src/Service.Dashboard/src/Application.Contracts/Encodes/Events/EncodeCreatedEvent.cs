using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;

public sealed record EncodeCreatedEvent : DomainEvent
{
    public required Guid EncodeId { get; init; }

    public required Guid FileId { get; init; }

    public required string FilePath { get; init; }
}