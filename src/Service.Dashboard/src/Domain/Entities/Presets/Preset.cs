using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Presets;

public class Preset : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public required string Container { get; set; }

    public required string EncodePreset { get; set; }

    public ICollection<PresetVideoStream>? VideoStreams { get; set; }

    public ICollection<PresetAudioStream>? AudioStreams { get; set; }

    public ICollection<PresetSubtitleStream>? SubtitleStreams { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}