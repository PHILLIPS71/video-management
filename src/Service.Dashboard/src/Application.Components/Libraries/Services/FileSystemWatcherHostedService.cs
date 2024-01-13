using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Services;

public class FileSystemWatcherHostedService : IHostedService
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

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _provider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();
            var libraries = await repository.ToListAsync(x => x.IsWatched, cancellationToken);

            foreach (var library in libraries)
            {
                _watcher.Watch(library);
            }

            var commands = libraries
                .Select(x => new LibraryScan.Command { LibraryId = x.Id })
                .ToList();

            await _bus.PublishBatch(commands, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}