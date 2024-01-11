namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;

public sealed record FileEncodeCancellationEvent
{
    public required Guid FileId { get; init; }

    public required Guid EncodeId { get; init; }
}