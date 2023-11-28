namespace Giantnodes.Infrastructure.Domain.Events;

public interface IEvent
{
    public DateTime RaisedAt { get; init; }
}