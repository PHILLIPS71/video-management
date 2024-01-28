namespace Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;

public sealed record EncodeCancelledEvent
{
    public required Guid EncodeId { get; init; }
}