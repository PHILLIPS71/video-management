namespace Giantnodes.Infrastructure.Uow.Execution.Impl;

public abstract class UnitOfWorkInterceptor : IUnitOfWorkInterceptor
{
    public abstract Task OnAfterCommitAsync(UnitOfWork uow, CancellationToken cancellation = default);
}