using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Services;

public class FileSystemWatcherHostedService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly IFileSystemWatcherService _watcher;
    private readonly IBus _bus;

    public FileSystemWatcherHostedService(
        IServiceProvider provider,
        IFileSystemWatcherService watcher,
        IBus bus)
    {
        _provider = provider;
        _watcher = watcher;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _provider.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IUnitOfWorkService>();
            var repository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();

            ICollection<Library>? libraries;
            using (var uow = await service.BeginAsync(stoppingToken))
            {
                libraries = await repository.ToListAsync(x => x.IsWatched, stoppingToken);

                var tasks = libraries
                    .Where(x => x.IsWatched)
                    .Select(x => x.Watch(_watcher))
                    .ToList();

                await Task.WhenAll(tasks);
                await uow.CommitAsync(stoppingToken);
            }

            var commands = libraries
                    .Where(x => x.Status != FileSystemStatus.Offline)
                    .Select(x => new LibraryScan.Command { LibraryId = x.Id })
                    .ToList();

            if (commands.Count > 0)
                await _bus.PublishBatch(commands, stoppingToken);
        }
    }
}