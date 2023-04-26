namespace Giantnodes.Infrastructure.Domain.Entities;

/// <summary>
/// Defines interface for base entity type.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>
    Guid Id { get; set; }
}