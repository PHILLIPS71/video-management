using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Domain.Values;
using MassTransit;
using FileStream = Giantnodes.Service.Dashboard.Domain.Values.FileStream;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Entities;

public class EncodeSnapshot : Entity<Guid>, ITimestampableEntity
{
    public Encode Encode { get; private set; }

    public long Size { get; private set; }

    public IReadOnlyCollection<VideoStream> VideoStreams { get; private set; } = new List<VideoStream>();

    public IReadOnlyCollection<AudioStream> AudioStreams { get; private set; } = new List<AudioStream>();

    public IReadOnlyCollection<SubtitleStream> SubtitleStreams { get; private set; } = new List<SubtitleStream>();

    public DateTime ProbedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private EncodeSnapshot()
    {
    }

    public EncodeSnapshot(Encode encode, long size, DateTime timestamp, params FileStream[] streams)
    {
        Id = NewId.NextSequentialGuid();
        Encode = encode;
        Size = size;
        ProbedAt = timestamp.ToUniversalTime();
        VideoStreams = VideoStreams
            .Union(streams.OfType<VideoStream>())
            .Intersect(streams.OfType<VideoStream>())
            .ToList();
        AudioStreams = AudioStreams
            .Union(streams.OfType<AudioStream>())
            .Intersect(streams.OfType<AudioStream>())
            .ToList();
        SubtitleStreams = SubtitleStreams
            .Union(streams.OfType<SubtitleStream>())
            .Intersect(streams.OfType<SubtitleStream>())
            .ToList();
    }
}