using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Infrastructure.Modules.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Giantnodes module system to the <paramref name="collection" />, and allows other
    /// Giantnodes modules to be configured
    /// </summary>
    /// <param name="collection">The <see cref="IServiceCollection"/> to add the module context to.</param>
    /// <param name="configure">The delegate that encapsulates the configuration of additional modules.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddGiantnodes(
        this IServiceCollection collection,
        Action<IModuleContext>? configure = default)
    {
        var configurator = new ModuleContextConfigurator(collection);
        configure?.Invoke(configurator);

        return collection;
    }
}