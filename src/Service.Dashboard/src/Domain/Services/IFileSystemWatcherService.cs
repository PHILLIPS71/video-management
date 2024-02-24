using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Dashboard.Domain.Services;

public interface IFileSystemWatcherService : IApplicationService, ISingletonDependency
{
    /// <summary>
    /// Attempts to asynchronously watch the specified path for file system changes and associates it with a custom
    /// event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to be raised when file system changes occur.</typeparam>
    /// <param name="path">The path to watch for file system changes.</param>
    /// <param name="raise">A function to convert <see cref="FileSystemEventArgs"/> to the custom event type.</param>
    /// <returns>
    /// A task representing the success of the watching operation. 
    /// The task result indicates whether the watching was successful (true) or unsuccessful (false).
    /// </returns>
    public Task<bool> TryWatchAsync<TEvent>(string path, Func<FileSystemEventArgs, TEvent> raise) where TEvent : class;

    /// <summary>
    /// Attempts to stop watching the specified path for file system changes.
    /// </summary>
    /// <param name="path">The path to stop watching.</param>
    /// <returns>
    /// A boolean indicating whether the un-watching operation was successful (true) or unsuccessful (false).
    /// </returns>
    public bool TryUnwatch(string path);
}