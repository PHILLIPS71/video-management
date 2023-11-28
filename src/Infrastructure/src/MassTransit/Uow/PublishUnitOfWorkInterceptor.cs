using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Uow;
using Giantnodes.Infrastructure.Uow.Execution;
using MassTransit;

namespace Giantnodes.Infrastructure.MassTransit.Uow;

public class PublishUnitOfWorkInterceptor : IUnitOfWorkInterceptor, IScopedDependency
{
    private readonly IBus _bus;

    public PublishUnitOfWorkInterceptor(IBus bus)
    {
        _bus = bus;
    }

    public async Task OnAfterCommitAsync(UnitOfWork uow, CancellationToken cancellation = default)
    {
        if (uow.Events.Count == 0)
            return;

        await _bus.PublishBatch(uow.Events, cancellation);
    }
}