using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;
using Giantnodes.Service.Dashboard.Domain.Services;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;

public class LibraryMonitoringService : ILibraryMonitoringService
{
    private readonly IFileSystem _fs;
    private readonly IFileSystemWatcherService _watcher;

    public LibraryMonitoringService(IFileSystem fs, IFileSystemWatcherService watcher)
    {
        _fs = fs;
        _watcher = watcher;
    }

    public async Task<bool> TryMonitorAsync(Library library)
    {
        var success = await _watcher.TryWatchAsync(
            library.PathInfo.FullName,
            @event => new LibraryFileSystemChangedEvent
            {
                LibraryId = library.Id,
                ChangeTypes = @event.ChangeType,
                FilePath = @event.FullPath,
            });

        var status = success ? FileSystemStatus.Online : FileSystemStatus.Offline;
        library.SetStatus(status);

        if (success)
            library.Scan(_fs);

        return success;
    }

    public bool TryUnMonitor(Library library)
    {
        var success = _watcher.TryUnwatch(library.PathInfo.FullName);

        var status = success ? FileSystemStatus.Online : FileSystemStatus.Offline;
        library.SetStatus(status);

        return success;
    }
}