namespace Giantnodes.Infrastructure.Domain.Entities;

/// <summary>
/// An entity that tracks when an entity was deleted, but does not remove the data from
/// the data source.
/// </summary>
public interface ISoftDeletableEntity
{
    /// <summary>
    /// The date and time when this entity was deleted.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}