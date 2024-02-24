namespace Giantnodes.Infrastructure.Messages;

public abstract record Message
{
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
}