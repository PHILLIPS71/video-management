using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeStartedEvent : IntegrationEvent
{
    public required string InputFilePath { get; init; }

    public required string OutputFilePath { get; init; }
}