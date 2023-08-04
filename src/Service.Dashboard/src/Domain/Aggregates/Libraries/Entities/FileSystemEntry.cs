using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public abstract class FileSystemEntry : Entity<Guid>
{
    public PathInfo PathInfo { get; private set; } = null!;

    public FileSystemDirectory? ParentDirectory { get; private set; }

    protected FileSystemEntry()
    {
    }

    protected FileSystemEntry(IFileSystemInfo entry)
    {
        PathInfo = new PathInfo(entry);
    }

    public void SetParentDirectory(FileSystemDirectory? directory)
    {
        if (PathInfo.DirectoryPath != directory?.PathInfo.FullName)
            throw new ArgumentException($"'{PathInfo.DirectoryPath}' is not a parent directory of '{directory?.PathInfo.FullName}'");

        ParentDirectory = directory;
    }
}