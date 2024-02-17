using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeCancelledEvent : CorrelatedBy<Guid>
{
    public required Guid CorrelationId { get; init; }
}