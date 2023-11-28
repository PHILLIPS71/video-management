using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Infrastructure.DependencyInjection.Configuration;

public interface IServiceCollectionConfigurator
{
    IServiceCollection Services { get; }
}