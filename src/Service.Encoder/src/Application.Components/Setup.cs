using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Encoder.Application.Components;

public static class Setup
{
    public static IServiceCollection SetupApplicationComponents(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystem, FileSystem>();

        return services;
    }
}