using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public class EncodeCancelledEvent : CorrelatedBy<Guid>
{
    public required Guid CorrelationId { get; init; }
}