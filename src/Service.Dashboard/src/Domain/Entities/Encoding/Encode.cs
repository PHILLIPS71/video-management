using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Entities.Presets;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Encoding;

public class Encode : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public required Guid PresetId { get; set; }
    public Preset? Preset { get; set; }

    public required string FullPath { get; set; }

    public EncodeStatus Status { get; set; } = EncodeStatus.Submitted;

    public int Percent { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime? FailedAt { get; set; }

    public string? FailedReason { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}