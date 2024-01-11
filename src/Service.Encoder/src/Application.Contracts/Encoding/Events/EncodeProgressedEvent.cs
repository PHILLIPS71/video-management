using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeProgressedEvent : CorrelatedBy<Guid>
{
    public required Guid CorrelationId { get; init; }

    public required float Percent { get; init; }
}