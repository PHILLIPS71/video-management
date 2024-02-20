using Giantnodes.Infrastructure.Domain.Events;
using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;

public sealed record FileProbeFaultedEvent : IntegrationEvent
{
    public required Guid JobId { get; init; }

    public required string FilePath { get; init; }

    public required ExceptionInfo Exception { get; init; }
}