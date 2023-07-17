using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public abstract class FileSystemEntry : Entity<Guid>
{
    public PathInfo PathInfo { get; private set; }

    public FileSystemDirectory? ParentDirectory { get; private set; }

    protected FileSystemEntry()
    {
    }

    protected FileSystemEntry(IFileSystemInfo node)
    {
        PathInfo = new PathInfo(node);
    }

    public void SetParentDirectory(FileSystemDirectory? directory)
    {
        if (PathInfo.DirectoryPath != directory?.PathInfo.FullName)
            throw new ArgumentException();

        ParentDirectory = directory;
    }
}