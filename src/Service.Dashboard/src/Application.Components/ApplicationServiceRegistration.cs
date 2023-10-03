using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Components.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.Application.Components;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystem, FileSystem>();
        services.TryAddSingleton<IFileSystemService, FileSystemService>();
        services.TryAddSingleton<IFileSystemWatcherFactory, FileSystemWatcherFactory>();

        services.TryAddSingleton<IFileSystemWatcherService, FileSystemWatcherService>();

        return services;
    }
}