using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Entities.Presets.Streams;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Presets;

public class Preset : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public required string Container { get; set; }

    public required string EncodePreset { get; set; }

    public ICollection<VideoStreamPreset>? VideoStreams { get; set; }

    public ICollection<AudioStreamPreset>? AudioStreams { get; set; }

    public ICollection<SubtitleStreamPreset>? SubtitleStreams { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}