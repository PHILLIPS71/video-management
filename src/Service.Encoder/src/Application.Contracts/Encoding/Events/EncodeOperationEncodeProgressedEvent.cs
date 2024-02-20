using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeOperationEncodeProgressedEvent : IntegrationEvent
{
    public required Guid JobId { get; init; }

    public required float Percent { get; init; }
}