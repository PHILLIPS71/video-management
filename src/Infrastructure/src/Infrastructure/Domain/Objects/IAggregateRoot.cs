﻿namespace Giantnodes.Infrastructure.Domain.Objects;

/// <summary>
/// A aggregate root where it's primary key is make up by a combination of keys.
/// </summary>
public interface IAggregateRoot : IEntity
{
}

/// <summary>
/// A aggregate root with a single primary key property.
/// </summary>
/// <typeparam name="TKey">Type of primary key property for the entity.</typeparam>
public interface IAggregateRoot<out TKey> : IEntity<TKey>
{
}