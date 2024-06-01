using System.IO.Abstractions;
using System.Reactive.Linq;
using Giantnodes.Service.Orchestrator.Domain.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Giantnodes.Service.Orchestrator.Infrastructure.Services;

public class FileSystemWatcherService : IFileSystemWatcherService
{
    private readonly Dictionary<string, IFileSystemWatcher> _watching = new();
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    private readonly IFileSystemWatcherFactory _factory;
    private readonly IFileSystemService _fileSystemService;
    private readonly IBus _bus;
    private readonly ILogger<FileSystemWatcherService> _logger;

    public FileSystemWatcherService(
        IFileSystemWatcherFactory factory,
        IFileSystemService fileSystemService,
        IBus bus,
        ILogger<FileSystemWatcherService> logger)
    {
        _factory = factory;
        _fileSystemService = fileSystemService;
        _bus = bus;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> TryWatchAsync<TEvent>(string path, Func<FileSystemEventArgs, TEvent> raise)
        where TEvent : class
    {
        try
        {
            if (_watching.ContainsKey(path))
                return true;

            var watcher = await Create(path, raise);
            _watching.Add(path, watcher);
            return true;
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.LogWarning(ex, "cannot watch path {Path} as it cannot be found or accessed.", path);
            return false;
        }
    }

    /// <inheritdoc />
    public bool TryUnwatch(string path)
    {
        if (!_watching.TryGetValue(path, out var watcher))
            return false;

        watcher.EnableRaisingEvents = false;
        _watching.Remove(path);
        return true;
    }

    /// <summary>
    /// Creates and configures a new file system watcher for the specified path, associated with a custom event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to be raised when file system changes occur.</typeparam>
    /// <param name="path">The path to watch for file system changes.</param>
    /// <param name="raise">A function to convert <see cref="FileSystemEventArgs"/> to the custom event type.</param>
    /// <returns>A task that represents the created file system watcher.</returns>
    private async Task<IFileSystemWatcher> Create<TEvent>(string path, Func<FileSystemEventArgs, TEvent> raise)
        where TEvent : class
    {
        var exists = await _fileSystemService.Exists(path);
        if (!exists)
            throw new DirectoryNotFoundException();

        var watcher = _factory.New(path, "*");
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        watcher.NotifyFilter = NotifyFilters.DirectoryName |
                               NotifyFilters.FileName |
                               NotifyFilters.LastWrite |
                               NotifyFilters.Size;

        // subscribe to file system events, convert them to the custom event type, and publish through the event bus.
        Observable
            .Merge(
                Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Created)),
                Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Renamed)),
                Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Deleted))
            )
            .Sample(_interval)
            .Subscribe(@event => _bus.Publish(raise(@event.EventArgs)));

        return watcher;
    }
}