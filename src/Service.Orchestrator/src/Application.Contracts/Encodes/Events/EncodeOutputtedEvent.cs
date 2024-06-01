using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Encodes.Events;

public sealed record EncodeOutputtedEvent : DomainEvent
{
    public required Guid EncodeId { get; init; }

    public required string Output { get; init; }

    public required string FullOutput { get; init; }
}