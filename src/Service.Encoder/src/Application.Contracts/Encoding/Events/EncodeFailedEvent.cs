using Giantnodes.Infrastructure.Domain.Events;
using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeFailedEvent : IntegrationEvent
{
    public required ExceptionInfo Exceptions { get; init; }
}