using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Infrastructure.Domain.Objects;

namespace Giantnodes.Infrastructure.Domain.Entities;

/// <inheritdoc cref="IAggregateRoot" />
public abstract class AggregateRoot : Entity,
    IAggregateRoot,
    IHasConcurrencyToken
{
    public virtual byte[]? ConcurrencyToken { get; private set; }
}

/// <inheritdoc cref="IAggregateRoot{TKey}" />
public abstract class AggregateRoot<TKey> : Entity<TKey>,
    IAggregateRoot,
    IHasConcurrencyToken
{
    public virtual byte[]? ConcurrencyToken { get; private set; }
}