using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Libraries;

public class Library : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }

    public required string Slug { get; set; }

    public required string FullPath { get; set; }

    public DriveStatus DriveStatus { get; set; } = DriveStatus.Online;
    
    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}