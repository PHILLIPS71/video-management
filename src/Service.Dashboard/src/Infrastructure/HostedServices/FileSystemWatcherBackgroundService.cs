using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Giantnodes.Service.Dashboard.Infrastructure.HostedServices;

public class FileSystemWatcherBackgroundService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILibraryMonitoringService _monitoring;

    public FileSystemWatcherBackgroundService(IServiceProvider provider, ILibraryMonitoringService monitoring)
    {
        _provider = provider;
        _monitoring = monitoring;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _provider.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IUnitOfWorkService>();
            var repository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();

            using (var uow = await service.BeginAsync(stoppingToken))
            {
                var libraries = await repository.ToListAsync(x => x.IsWatched, stoppingToken);

                var tasks = libraries
                    .Where(library => library.IsWatched)
                    .Select(library => _monitoring.TryMonitorAsync(library))
                    .ToList();

                await Task.WhenAll(tasks);
                await uow.CommitAsync(stoppingToken);
            }
        }
    }
}