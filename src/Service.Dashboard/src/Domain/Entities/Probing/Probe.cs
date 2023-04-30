using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Probing;

public class Probe : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }
    
    public required string FullPath { get; set; }
    
    public ProbeStatus Status { get; set; } = ProbeStatus.Submitted;
    
    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime? FailedAt { get; set; }

    public string? FailedReason { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
}