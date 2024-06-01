using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Encodes.Events;

public sealed record EncodeCancelledEvent : DomainEvent
{
    public required Guid EncodeId { get; init; }
}