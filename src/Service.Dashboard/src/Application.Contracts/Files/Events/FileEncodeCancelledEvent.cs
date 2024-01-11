namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;

public sealed record FileEncodeCancelledEvent
{
    public required Guid FileId { get; init; }

    public required Guid EncodeId { get; init; }
}