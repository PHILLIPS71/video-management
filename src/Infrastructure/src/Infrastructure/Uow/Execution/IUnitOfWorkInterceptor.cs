namespace Giantnodes.Infrastructure.Uow.Execution;

public interface IUnitOfWorkInterceptor
{
    Task OnAfterCommitAsync(UnitOfWork uow, CancellationToken cancellation = default);
}