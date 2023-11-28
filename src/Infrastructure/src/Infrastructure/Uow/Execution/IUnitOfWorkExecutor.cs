namespace Giantnodes.Infrastructure.Uow.Execution;

public interface IUnitOfWorkExecutor
{
    Task OnAfterCommitAsync(UnitOfWork uow, CancellationToken cancellation = default);
}