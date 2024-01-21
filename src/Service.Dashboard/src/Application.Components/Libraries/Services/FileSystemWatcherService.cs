using System.IO.Abstractions;
using System.Reactive.Linq;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using MassTransit;
using Serilog;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Services;

public class FileSystemWatcherService : IFileSystemWatcherService
{
    private readonly Dictionary<Guid, IFileSystemWatcher> _libraries = new();
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
    public async Task<bool> TryWatch(Library library)
    {
        try
        {
            if (_libraries.ContainsKey(library.Id))
                return true;

            var watcher = await Create(library);
            _libraries.Add(library.Id, watcher);
            return true;
        }
        catch (DirectoryNotFoundException)
        {
            Log.Warning("Cannot watch library {0} at {1} as it cannot be found or accessed.", library.Name, library.PathInfo.FullName);
            return false;
        }
    }

    /// <inheritdoc />
    public bool TryUnwatch(Library library)
    {
        if (!_libraries.TryGetValue(library.Id, out var watcher))
            return false;

        watcher.EnableRaisingEvents = false;
        _libraries.Remove(library.Id);
        return true;
    }

    private async Task<IFileSystemWatcher> Create(Library library)
    {
        var exists = await _fileSystemService.Exists(library.PathInfo.FullName);
        if (!exists)
            throw new DirectoryNotFoundException();

        var watcher = _factory.New(library.PathInfo.FullName, "*");
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        watcher.NotifyFilter = NotifyFilters.DirectoryName |
                               NotifyFilters.FileName |
                               NotifyFilters.LastWrite |
                               NotifyFilters.Size;

        Observable
            .Merge(
                Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Created)),
                Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Renamed)),
                Observable.FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Deleted))
            )
            .Sample(_interval)
            .Subscribe(_ => _bus.Publish(new LibraryScan.Command { LibraryId = library.Id }));

        return watcher;
    }
}