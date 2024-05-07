using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeOperationOutputtedEvent : IntegrationEvent
{
    public required Guid JobId { get; init; }

    public required string Data { get; init; }
}