using Giantnodes.Infrastructure.Domain.Events;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;

public sealed record EncodeStatusChangedEvent : DomainEvent
{
    public required Guid EncodeId { get; init; }

    public required EncodeStatus FromStatus { get; init; }

    public required EncodeStatus ToStatus { get; init; }
}