using System.IO.Abstractions;
using Giantnodes.Infrastructure.DependencyInjection.Extensions;
using Giantnodes.Infrastructure.EntityFrameworkCore.Uow;
using Giantnodes.Infrastructure.MassTransit.Uow;
using Giantnodes.Infrastructure.Uow.DependencyInjection;
using Giantnodes.Service.Dashboard.Application.Components.Directories.Repositories;
using Giantnodes.Service.Dashboard.Application.Components.Entries.Repositories;
using Giantnodes.Service.Dashboard.Application.Components.Files.Repositories;
using Giantnodes.Service.Dashboard.Application.Components.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Application.Components.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.Application.Components;

public static class Setup
{
    public static IServiceCollection SetupApplicationComponents(this IServiceCollection services)
    {
        services.AddGiantnodes(options =>
        {
            options.UsingUow(configure =>
            {
                configure
                    .TryAddProvider<EntityFrameworkUnitOfWork<ApplicationDbContext>>()
                    .TryAddInterceptor<PublishUnitOfWorkInterceptor>();
            });
        });

        services.TryAddSingleton<IFileSystem, FileSystem>();
        services.TryAddSingleton<IFileSystemService, FileSystemService>();
        services.TryAddSingleton<IFileSystemWatcherFactory, FileSystemWatcherFactory>();

        services.TryAddSingleton<IFileSystemWatcherService, FileSystemWatcherService>();

        services.TryAddTransient<ILibraryRepository, LibraryRepository>();
        services.TryAddTransient<IFileSystemEntryRepository, FileSystemEntryRepository>();
        services.TryAddTransient<IFileSystemDirectoryRepository, FileSystemDirectoryRepository>();
        services.TryAddTransient<IFileSystemFileRepository, FileSystemFileRepository>();

        services.AddHostedService<FileSystemWatcherHostedService>();

        return services;
    }
}