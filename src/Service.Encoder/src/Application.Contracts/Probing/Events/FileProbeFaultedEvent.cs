﻿using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;

public sealed record FileProbeFaultedEvent
{
    public required Guid JobId { get; init; }

    public required string Path { get; init; }

    public required ExceptionInfo Exception { get; init; }

    public required DateTime Timestamp { get; init; }
}