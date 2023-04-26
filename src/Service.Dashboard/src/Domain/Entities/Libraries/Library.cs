using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Libraries;

public class Library : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string FullPath { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}