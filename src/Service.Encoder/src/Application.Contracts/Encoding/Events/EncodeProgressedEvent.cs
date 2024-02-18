using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeProgressedEvent : IntegrationEvent
{
    public required float Percent { get; init; }
}