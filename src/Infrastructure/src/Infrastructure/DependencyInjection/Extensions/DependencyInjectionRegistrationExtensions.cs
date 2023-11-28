using Giantnodes.Infrastructure.DependencyInjection.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Infrastructure.DependencyInjection.Extensions;

public static class DependencyInjectionRegistrationExtensions
{
    public static IServiceCollection AddGiantnodes(
        this IServiceCollection collection,
        Action<IServiceCollectionConfigurator> configure)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var configurator = new ServiceCollectionConfigurator(collection);
        configure.Invoke(configurator);

        return collection;
    }
}