namespace Giantnodes.Infrastructure.Domain.Entities;

/// <summary>
/// An entity that tracks the <see cref="CreatedAt" /> and <see cref="UpdatedAt" /> times when stored in the
/// database. These values are automatically set when saving <see cref="IEntity"/> to the database.
/// </summary>
public interface ITimestampableEntity
{
    /// <summary>
    /// The date and time when this entity was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// The date and time when this entity was created.
    /// </summary>
    DateTime CreatedAt { get; set; }
}