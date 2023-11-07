namespace Giantnodes.Infrastructure.Uow;

public interface IUnitOfWorkContext : IDisposable
{
    Guid CorrelationId { get; }

    Task CommitAsync(CancellationToken cancellation = default);
}