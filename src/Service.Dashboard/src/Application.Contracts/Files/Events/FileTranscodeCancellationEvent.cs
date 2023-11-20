namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;

public sealed record FileTranscodeCancellationEvent
{
    public required Guid FileId { get; init; }

    public required Guid TranscodeId { get; init; }
}