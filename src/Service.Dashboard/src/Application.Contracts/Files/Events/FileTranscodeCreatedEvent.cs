namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;

public sealed record FileTranscodeCreatedEvent
{
    public required Guid FileId { get; init; }

    public required Guid TranscodeId { get; init; }

    public required string FullPath { get; init; }
}