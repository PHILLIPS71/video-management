﻿namespace Giantnodes.Infrastructure.Domain.Entities.Auditing;

/// <summary>
/// An entity that tracks the <see cref="CreatedAt" /> and <see cref="UpdatedAt" /> times of when the entity was
/// created or updated.
/// </summary>
public interface ITimestampableEntity
{
    /// <summary>
    /// The date and time when this entity was created.
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// The date and time when this entity was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }
}