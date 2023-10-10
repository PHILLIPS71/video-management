using Giantnodes.Service.Encoder.Application.Contracts.Common;

namespace Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;

public sealed record FileProbedEvent
{
    public required Guid JobId { get; init; }

    public required string FullPath { get; init; }

    public required string Name { get; init; }

    public required long Size { get; init; }

    public required DateTime Timestamp { get; init; }

    public required FileVideoStream[] VideoStreams { get; init; } = Array.Empty<FileVideoStream>();

    public required FileAudioStream[] AudioStreams { get; init; } = Array.Empty<FileAudioStream>();

    public required FileSubtitleStream[] SubtitleStreams { get; init; } = Array.Empty<FileSubtitleStream>();
}