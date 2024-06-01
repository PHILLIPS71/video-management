using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries.Services;

public interface ILibraryMonitoringService : IDomainService, ISingletonDependency
{
    /// <summary>
    /// Attempts to asynchronously monitor the specified library for file system changes using a watcher.
    /// </summary>
    /// <param name="library">The library to monitor.</param>
    /// <returns>
    /// A task representing the success of the monitoring operation. The task result indicates whether the monitoring
    /// was successful (true) or unsuccessful (false).
    /// </returns>
    public Task<bool> TryMonitorAsync(Library library);

    /// <summary>
    /// Attempts to stop monitoring the specified library for file system changes.
    /// </summary>
    /// <param name="library">The library to stop monitoring.</param>
    /// <returns>
    /// A boolean indicating whether the un-monitoring operation was successful (true) or unsuccessful (false).
    /// </returns>
    public bool TryUnMonitor(Library library);
}