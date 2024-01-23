using System.IO.Abstractions;
using System.Security;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Domain.Values;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;

public class Library : AggregateRoot<Guid>, ITimestampableEntity
{
    private readonly List<FileSystemEntry> _entries = new();

    public string Name { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public PathInfo PathInfo { get; private set; } = null!;

    public FileSystemStatus Status { get; private set; } = FileSystemStatus.Offline;

    public bool IsWatched { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public FileSystemDirectory Directory => _entries
        .OfType<FileSystemDirectory>()
        .Single(x => x.PathInfo.FullName == PathInfo.FullName);

    public IReadOnlyCollection<FileSystemEntry> Entries => _entries.AsReadOnly();

    private Library()
    {
    }

    public Library(IDirectoryInfo root, string name, string slug)
    {
        Id = NewId.NextSequentialGuid();
        Name = name;
        Slug = slug;
        PathInfo = new PathInfo(root);

        if (root.Exists)
            Status = FileSystemStatus.Online;

        _entries.Add(new FileSystemDirectory(this, null, root));
    }

    public void SetStatus(FileSystemStatus status)
    {
        Status = status;
    }

    public void SetWatched(bool watched)
    {
        IsWatched = watched;

        DomainEvents.Add(new LibraryMonitoringChangedEvent
        {
            LibraryId = Id,
            IsMonitoring = IsWatched,
            RaisedAt = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Traverses the <see cref="Directory"/> and any sub-directories within it, creating or updating any
    /// existing <see cref="Entries"/> as well as removing those that no longer exist.
    /// </summary>
    /// <param name="fs">A file system abstraction where the library exists.</param>
    public void Scan(IFileSystem fs)
    {
        var parent = Directory;
        var paths = new List<string> { parent.PathInfo.FullName };

        var stack = new Stack<string>();
        stack.Push(Directory.PathInfo.FullName);

        Status = FileSystemStatus.Online;

        while (stack.Count > 0)
        {
            var path = stack.Pop();
            parent = _entries.OfType<FileSystemDirectory>().Single(x => x.PathInfo.FullName == path);

            try
            {
                var infos = fs.GetVideoFiles(path);

                foreach (var info in infos)
                {
                    var entry = _entries.SingleOrDefault(x => x.PathInfo.FullName == info.FullName);
                    if (entry == null)
                    {
                        entry = FileSystemEntry.Build(this, parent, info);
                        _entries.Add(entry);
                    }

                    switch (info)
                    {
                        case IFileInfo file:
                            ((FileSystemFile)entry).SetSize(file);
                            break;

                        case IDirectoryInfo subdirectory:
                            ((FileSystemDirectory)entry).SetSize(fs);
                            stack.Push(subdirectory.FullName);
                            break;
                    }

                    entry.SetScannedAt(DateTime.UtcNow);
                    paths.Add(info.FullName);
                }
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException or SecurityException or IOException)
                    Status = FileSystemStatus.Degraded;

                else if (ex is DirectoryNotFoundException)
                    Status = FileSystemStatus.Offline;

                else
                    throw;
            }
        }

        _entries.RemoveAll(x => paths.TrueForAll(path => path != x.PathInfo.FullName));
    }
}