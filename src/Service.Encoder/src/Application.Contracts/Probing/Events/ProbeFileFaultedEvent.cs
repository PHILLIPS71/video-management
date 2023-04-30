using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;

public sealed record ProbeFileFaultedEvent
{
    public required Guid JobId { get; init; }

    public required string FullPath { get; init; }

    public required ExceptionInfo Exception { get; init; }

    public required DateTime Timestamp { get; init; }
}