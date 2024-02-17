using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeCompletedEvent : CorrelatedBy<Guid>
{
    public required Guid CorrelationId { get; init; }

    public required string InputFilePath { get; init; }

    public required string OutputFilePath { get; init; }
}