using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Presets.Streams;

public class SubtitleStreamPreset : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public Guid PresetId { get; set; }
    public Preset? Preset { get; set; }

    public required string Codec { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}