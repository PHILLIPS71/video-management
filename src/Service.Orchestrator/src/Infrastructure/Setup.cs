using System.IO.Abstractions;
using Giantnodes.Infrastructure.DependencyInjection.Extensions;
using Giantnodes.Infrastructure.EntityFrameworkCore.Uow;
using Giantnodes.Infrastructure.MassTransit.Uow;
using Giantnodes.Infrastructure.Uow.DependencyInjection;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Services;
using Giantnodes.Service.Orchestrator.Infrastructure.HostedServices;
using Giantnodes.Service.Orchestrator.Infrastructure.Repositories;
using Giantnodes.Service.Orchestrator.Infrastructure.Services;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Orchestrator.Infrastructure;

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
        services.TryAddTransient<IFileSystemDirectoryRepository, FileSystemDirectoryRepository>();
        services.TryAddTransient<IRecipeRepository, RecipeRepository>();
        services.TryAddTransient<IEncodeRepository, EncodeRepository>();
        services.TryAddTransient<IFileSystemEntryRepository, FileSystemEntryRepository>();
        services.TryAddTransient<IFileSystemFileRepository, FileSystemFileRepository>();
        services.TryAddTransient<ILibraryRepository, LibraryRepository>();

        // Services
        services.TryAddSingleton<IFileSystemService, FileSystemService>();
        services.TryAddSingleton<IFileSystemWatcherService, FileSystemWatcherService>();

        // Background Services
        services.AddHostedService<FileSystemWatcherBackgroundService>();

        return services;
    }
}