using System.IO.Abstractions;
using Giantnodes.Infrastructure.DependencyInjection.Extensions;
using Giantnodes.Infrastructure.EntityFrameworkCore.Uow;
using Giantnodes.Infrastructure.MassTransit.Uow;
using Giantnodes.Infrastructure.Uow.DependencyInjection;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Services;
using Giantnodes.Service.Dashboard.Infrastructure.Aggregates.Directories;
using Giantnodes.Service.Dashboard.Infrastructure.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Infrastructure.Aggregates.Entries;
using Giantnodes.Service.Dashboard.Infrastructure.Aggregates.Files;
using Giantnodes.Service.Dashboard.Infrastructure.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Infrastructure.HostedServices;
using Giantnodes.Service.Dashboard.Infrastructure.Services;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.Infrastructure;

public static class Setup
{
    public static IServiceCollection SetupInfrastructure(this IServiceCollection services)
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

        // System.IO.Abstractions
        services.TryAddSingleton<IFileSystem, FileSystem>();
        services.TryAddSingleton<IFileSystemWatcherFactory, FileSystemWatcherFactory>();

        // Repositories
        services.TryAddTransient<ILibraryRepository, LibraryRepository>();
        services.TryAddTransient<IFileSystemEntryRepository, FileSystemEntryRepository>();
        services.TryAddTransient<IFileSystemDirectoryRepository, FileSystemDirectoryRepository>();
        services.TryAddTransient<IFileSystemFileRepository, FileSystemFileRepository>();
        services.TryAddTransient<IEncodeRepository, EncodeRepository>();

        // Services
        services.TryAddSingleton<IFileSystemService, FileSystemService>();
        services.TryAddSingleton<IFileSystemWatcherService, FileSystemWatcherService>();

        // Background Services
        services.AddHostedService<FileSystemWatcherBackgroundService>();

        return services;
    }
}