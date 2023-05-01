using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Presets;

public class PresetVideoStream : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public Guid EncodePresetId { get; set; }
    public Preset? EncodePreset { get; set; }

    public required string Codec { get; set; }

    public int? Height { get; set; }

    public int? Width { get; set; }

    public long? Bitrate { get; set; }

    public double? Framerate { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}