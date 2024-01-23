using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Domain.Services;
using MassTransit;
using Serilog;

namespace Giantnodes.Service.Dashboard.Infrastructure.Services;

public class FileSystemWatcherService : IFileSystemWatcherService
{
    private readonly Dictionary<string, IFileSystemWatcher> _watching = new();
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    private readonly IFileSystemWatcherFactory _factory;
    private readonly IFileSystemService _fileSystemService;
    private readonly IBus _bus;

    public FileSystemWatcherService(IFileSystemWatcherFactory factory, IFileSystemService fileSystemService, IBus bus)
    {
        _factory = factory;
        _fileSystemService = fileSystemService;
        _bus = bus;
    }

    /// <inheritdoc />
    public async Task<bool> TryWatchAsync(string path)
    {
        try
        {
            if (_watching.ContainsKey(path))
                return true;

            var watcher = await Create(path);
            _watching.Add(path, watcher);
            return true;
        }
        catch (DirectoryNotFoundException)
        {
            Log.Warning("Cannot watch path {0} as it cannot be found or accessed.", path);
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

    private async Task<IFileSystemWatcher> Create(string path)
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

        // Observable
        //     .Merge(
        //         Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Created)),
        //         Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Renamed)),
        //         Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Deleted))
        //     )
        //     .Sample(_interval)
        //     .Subscribe(_ => _bus.Publish(new LibraryScan.Command { LibraryId = library.Id }));

        return watcher;
    }
}