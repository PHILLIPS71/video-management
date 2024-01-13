using System.IO.Abstractions;
using System.Reactive.Linq;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Services;

public class FileSystemWatcherService : IFileSystemWatcherService
{
    private readonly Dictionary<Guid, IFileSystemWatcher> _libraries = new();
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    private readonly IFileSystemWatcherFactory _factory;
    private readonly IBus _bus;

    public FileSystemWatcherService(IFileSystemWatcherFactory factory, IBus bus)
    {
        _factory = factory;
        _bus = bus;
    }

    /// <inheritdoc />
    public void Watch(Library library)
    {
        if (!_libraries.ContainsKey(library.Id))
            _libraries.Add(library.Id, Create(library));
    }

    /// <inheritdoc />
    public void Unwatch(Library library)
    {
        if (!_libraries.TryGetValue(library.Id, out var watcher))
            return;

        watcher.EnableRaisingEvents = false;
        _libraries.Remove(library.Id);
    }

    private IFileSystemWatcher Create(Library library)
    {
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