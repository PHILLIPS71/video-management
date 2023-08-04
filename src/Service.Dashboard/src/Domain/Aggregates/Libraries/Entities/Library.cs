using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Factories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class Library : AggregateRoot<Guid>
{
    private readonly List<FileSystemEntry> _entries;

    public string Name { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public PathInfo PathInfo { get; private set; } = null!;

    public DriveStatus DriveStatus { get; private set; } = DriveStatus.Offline;

    public IReadOnlyCollection<FileSystemEntry> Entries => _entries.AsReadOnly();

    protected Library()
    {
        _entries = new List<FileSystemEntry>();
    }

    public Library(IFileSystemInfo info, string name, string slug)
    {
        Name = name;
        Slug = slug;
        PathInfo = new PathInfo(info);
        DriveStatus = info.Exists ? DriveStatus.Online : DriveStatus.Offline;

        _entries = new List<FileSystemEntry>();
    }

    public void Scan(ILibraryService service)
    {
        try
        {
            var entries = service.GetFileSystemInfos(this);

            // remove any nodes that no longer exist within the scanned path
            _entries.RemoveAll(x => entries.All(y => y.FullName != x.PathInfo.FullName));

            foreach (var entry in entries)
            {
                var node = _entries.SingleOrDefault(x => x.PathInfo.FullName == entry.FullName);
                if (node == null)
                {
                    node = FileSystemEntryFactory.Build(entry);
                    _entries.Add(node);
                }

                switch (entry)
                {
                    case IFileInfo file:
                        ((FileSystemFile)node).SetSize(file);
                        break;
                }
            }

            DriveStatus = DriveStatus.Online;
        }
        catch (DirectoryNotFoundException)
        {
            DriveStatus = DriveStatus.Offline;
            throw;
        }
    }
}