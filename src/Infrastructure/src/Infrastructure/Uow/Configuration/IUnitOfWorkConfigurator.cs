using Giantnodes.Infrastructure.DependencyInjection.Configuration;
using Giantnodes.Infrastructure.Uow.Execution;

namespace Giantnodes.Infrastructure.Uow.Configuration;

public interface IUnitOfWorkConfigurator : IServiceCollectionConfigurator
{
    IUnitOfWorkConfigurator TryAddInterceptor<TInterceptor>()
        where TInterceptor : IUnitOfWorkInterceptor;

    IUnitOfWorkConfigurator TryAddProvider<TUnitOfWork>()
        where TUnitOfWork : IUnitOfWork;
}