using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

public interface IFileSystemWatcherService : IApplicationService, ISingletonDependency
{
    /// <summary>
    /// Starts watching the specified <paramref name="library" /> for file system changes.
    /// </summary>
    /// <param name="library">A <see cref="Library" /> to watch for file system changes.</param>
    /// <seealso cref="FileSystemWatcher.EnableRaisingEvents" />
    public void Watch(Library library);

    /// <summary>
    /// Stops watching the specified <paramref name="library" /> for file system changes.
    /// </summary>
    /// <param name="library">A <see cref="Library" /> to stop watching for file system changes.</param>
    public void Unwatch(Library library);
}