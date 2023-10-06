using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Values;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public abstract class FileSystemEntry : Entity<Guid>
{
    public long Size { get; protected set; }

    public PathInfo PathInfo { get; protected set; } = null!;

    public FileSystemDirectory? ParentDirectory { get; protected set; }

    public Library Library { get; protected set; }  = null!;

    protected FileSystemEntry()
    {
    }

    protected FileSystemEntry(FileSystemDirectory? parent, IFileSystemInfo entry)
    {
        Id = NewId.NextSequentialGuid();
        ParentDirectory = parent;
        PathInfo = new PathInfo(entry);
    }
}