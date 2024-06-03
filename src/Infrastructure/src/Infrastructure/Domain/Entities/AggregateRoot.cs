using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Infrastructure.Domain.Events;
using Giantnodes.Infrastructure.Domain.Objects;

namespace Giantnodes.Infrastructure.Domain.Entities;

/// <inheritdoc cref="IAggregateRoot" />
public abstract class AggregateRoot : Entity, IAggregateRoot, IHasConcurrencyToken
{
    public readonly ICollection<DomainEvent> DomainEvents = new List<DomainEvent>();

    public uint ConcurrencyToken { get; private set; }
}

/// <inheritdoc cref="IAggregateRoot{TKey}" />
public class AggregateRoot<TKey> : AggregateRoot, IAggregateRoot<TKey>
{
    public TKey Id { get; protected init; } = default!;

    public override object[] GetKeys()
    {
        return new object[] { Id };
    }
}