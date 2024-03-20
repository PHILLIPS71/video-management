using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;

public sealed record EncodeCancelledEvent : DomainEvent
{
    public required Guid EncodeId { get; init; }
}