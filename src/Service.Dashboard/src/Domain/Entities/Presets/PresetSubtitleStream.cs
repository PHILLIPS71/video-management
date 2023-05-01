using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Presets;

public class PresetSubtitleStream : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public Guid EncodePresetId { get; set; }
    public Preset? EncodePreset { get; set; }

    public required string Codec { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}