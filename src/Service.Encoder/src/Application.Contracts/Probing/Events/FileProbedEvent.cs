using Giantnodes.Infrastructure.Domain.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Common;

namespace Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;

public sealed record FileProbedEvent : IntegrationEvent
{
    public required Guid JobId { get; init; }

    public required string FilePath { get; init; }

    public required string Name { get; init; }

    public required long Size { get; init; }

    public required FileVideoStream[] VideoStreams { get; init; } = Array.Empty<FileVideoStream>();

    public required FileAudioStream[] AudioStreams { get; init; } = Array.Empty<FileAudioStream>();

    public required FileSubtitleStream[] SubtitleStreams { get; init; } = Array.Empty<FileSubtitleStream>();
}