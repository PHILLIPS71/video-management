using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Encodes.Events;

public sealed record EncodeProgressedEvent : DomainEvent
{
    public required Guid EncodeId { get; init; }

    public required float Percent { get; set; }
}