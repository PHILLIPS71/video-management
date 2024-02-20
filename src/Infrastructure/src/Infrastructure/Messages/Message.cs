namespace Giantnodes.Infrastructure.Messages;

public abstract record Message
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}