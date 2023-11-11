﻿using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Components.Directories.Repositories;
using Giantnodes.Service.Dashboard.Application.Components.Files.Repositories;
using Giantnodes.Service.Dashboard.Application.Components.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Application.Components.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.Application.Components;

public static class Setup
{
    public static IServiceCollection SetupApplicationComponents(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystem, FileSystem>();
        services.TryAddSingleton<IFileSystemService, FileSystemService>();
        services.TryAddSingleton<IFileSystemWatcherFactory, FileSystemWatcherFactory>();

        services.TryAddSingleton<IFileSystemWatcherService, FileSystemWatcherService>();

        services.TryAddTransient<ILibraryRepository, LibraryRepository>();
        services.TryAddTransient<IFileSystemDirectoryRepository, FileSystemDirectoryRepository>();
        services.TryAddTransient<IFileSystemFileRepository, FileSystemFileRepository>();

        return services;
    }
}