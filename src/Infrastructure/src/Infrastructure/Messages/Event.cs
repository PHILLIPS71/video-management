namespace Giantnodes.Infrastructure.Messages;

public abstract record Event : Message
{
    public DateTime RaisedAt { get; init; } = DateTime.UtcNow;
}