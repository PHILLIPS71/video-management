﻿using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public record EncodeStartedEvent : CorrelatedBy<Guid>
{
    public required Guid CorrelationId { get; init; }

    public required string InputPath { get; init; }

    public required string OutputPath { get; init; }
}