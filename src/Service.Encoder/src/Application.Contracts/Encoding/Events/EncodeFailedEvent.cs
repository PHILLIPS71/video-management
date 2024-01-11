using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public record EncodeFailedEvent : CorrelatedBy<Guid>
{
    public required Guid CorrelationId { get; init; }

    public required ExceptionInfo Exceptions { get; init; }
}