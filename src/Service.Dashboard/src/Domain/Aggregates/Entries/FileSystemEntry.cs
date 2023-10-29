using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Values;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Entries;

public abstract class FileSystemEntry : AggregateRoot<Guid>
{
    public Library Library { get; protected set; }
    
    public FileSystemDirectory? ParentDirectory { get; protected set; }

    public PathInfo PathInfo { get; protected set; }

    public long Size { get; protected set; }

    protected FileSystemEntry()
    {
    }
    
    protected FileSystemEntry(Library library, FileSystemDirectory? parent, IFileSystemInfo entry)
    {
        Id = NewId.NextSequentialGuid();
        Library = library;
        ParentDirectory = parent;
        PathInfo = new PathInfo(entry);
    }

    public static FileSystemEntry Build(Library library, FileSystemDirectory parent, IFileSystemInfo info, IFileSystemService service)
    {
        return info switch
        {
            IDirectoryInfo directory => new FileSystemDirectory(library, parent, directory, service),
            IFileInfo file => new FileSystemFile(library, parent, file),
            _ => throw new ArgumentOutOfRangeException(nameof(info), info, null)
        };
    }
}