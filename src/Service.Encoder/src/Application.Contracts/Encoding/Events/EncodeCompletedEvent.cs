using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeCompletedEvent : IntegrationEvent
{
    public required string InputFilePath { get; init; }

    public required string OutputFilePath { get; init; }
}