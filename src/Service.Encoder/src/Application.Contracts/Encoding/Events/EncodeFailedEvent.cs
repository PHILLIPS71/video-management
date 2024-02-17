using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeFailedEvent : CorrelatedBy<Guid>
{
    public required Guid CorrelationId { get; init; }

    public required ExceptionInfo Exceptions { get; init; }
}