using Giantnodes.Infrastructure.DependencyInjection;

namespace Giantnodes.Infrastructure.Uow.Execution.Impl;

public class UnitOfWorkExecutor : IUnitOfWorkExecutor, ISingletonDependency
{
    private readonly IEnumerable<IUnitOfWorkInterceptor> _interceptors;

    public UnitOfWorkExecutor(IEnumerable<IUnitOfWorkInterceptor> interceptors)
    {
        _interceptors = interceptors;
    }

    public Task OnAfterCommitAsync(UnitOfWork uow, CancellationToken cancellation = default)
    {
        return Task.WhenAll(_interceptors.Select(x => x.OnAfterCommitAsync(uow, cancellation)));
    }
}