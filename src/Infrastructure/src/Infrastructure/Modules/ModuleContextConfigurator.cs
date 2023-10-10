using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Infrastructure.Modules;

internal class ModuleContextConfigurator : ModuleContext
{
    public ModuleContextConfigurator(IServiceCollection collection)
        : base(collection)
    {
    }
}