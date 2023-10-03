using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Services;

public class FileSystemWatcherService : IFileSystemWatcherService
{
    private readonly Dictionary<Guid, IFileSystemWatcher> _libraries = new();

    private readonly IBus _bus;
    private readonly IFileSystemWatcherFactory _factory;

    public FileSystemWatcherService(IBus bus, IFileSystemWatcherFactory factory)
    {
        _bus = bus;
        _factory = factory;
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

        watcher.Created += async (_, @event) =>
            await _bus.Publish(new LibraryFileCreatedEvent { Id = library.Id, FullPath = @event.FullPath });

        watcher.Deleted += async (_, @event) =>
            await _bus.Publish(new LibraryFileDeletedEvent { Id = library.Id, FullPath = @event.FullPath });

        return watcher;
    }
}