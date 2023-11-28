using Giantnodes.Infrastructure.DependencyInjection.Configuration;
using Giantnodes.Infrastructure.Uow.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Infrastructure.Uow.Configuration;

public class UnitOfWorkConfigurator : ServiceCollectionConfigurator, IUnitOfWorkConfigurator
{
    public UnitOfWorkConfigurator(IServiceCollection collection)
        : base(collection)
    {
    }

    public IUnitOfWorkConfigurator TryAddInterceptor<TInterceptor>()
        where TInterceptor : IUnitOfWorkInterceptor
    {
        Services.TryAddScoped(typeof(IUnitOfWorkInterceptor), typeof(TInterceptor));

        return this;
    }

    public IUnitOfWorkConfigurator TryAddProvider<TUnitOfWork>()
        where TUnitOfWork : IUnitOfWork
    {
        Services.TryAddTransient(typeof(IUnitOfWork), typeof(TUnitOfWork));

        return this;
    }
}