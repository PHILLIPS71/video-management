using Giantnodes.Infrastructure.Domain.Objects;

namespace Giantnodes.Infrastructure.Domain.Entities;

/// <inheritdoc/>
public abstract class Entity : IEntity
{
    public abstract object[] GetKeys();
}

/// <inheritdoc cref="IEntity{TKey}" />
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    /// <inheritdoc/>
    public virtual TKey Id { get; protected set; } = default!;

    public override object[] GetKeys()
    {
        return new object[] { Id };
    }
}