﻿namespace Giantnodes.Infrastructure.Domain.Entities.Auditing;

/// <summary>
/// An entity that tracks its version to prevent concurrency conflicts via optimistic concurrency.
/// </summary>
public interface IHasConcurrencyToken
{
    /// <summary>
    /// A concurrency token used as a version of the entity.
    /// </summary>
    byte[]? ConcurrencyToken { get; }
}