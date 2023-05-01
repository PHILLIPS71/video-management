using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Presets;

public class PresetAudioStream : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public Guid EncodePresetId { get; set; }
    public Preset? EncodePreset { get; set; }

    public required string Codec { get; set; }

    public required int Channels { get; init; }
    
    public long? Bitrate { get; init; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}