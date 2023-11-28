using Giantnodes.Infrastructure.DependencyInjection.Configuration;
using Giantnodes.Infrastructure.Uow.Configuration;
using Giantnodes.Infrastructure.Uow.Execution;
using Giantnodes.Infrastructure.Uow.Execution.Impl;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Infrastructure.Uow.Services.Impl;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Infrastructure.Uow.DependencyInjection;

public static class UnitOfWorkConfiguratorExtensions
{
    public static IServiceCollectionConfigurator UsingUow(
        this IServiceCollectionConfigurator collection,
        Action<IUnitOfWorkConfigurator> configure)
    {
        collection.Services.TryAddTransient<IUnitOfWorkService, UnitOfWorkService>();

        collection.Services.TryAddScoped<IUnitOfWorkExecutor, UnitOfWorkExecutor>();

        var configurator = new UnitOfWorkConfigurator(collection.Services);
        configure.Invoke(configurator);
        
        return configurator;
    }
}