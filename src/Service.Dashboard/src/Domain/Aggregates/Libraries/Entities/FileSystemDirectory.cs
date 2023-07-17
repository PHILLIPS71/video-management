using System.IO.Abstractions;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class FileSystemDirectory : FileSystemEntry
{
    // public IReadOnlyCollection<FileSystemNode> Children { get; init; }

    protected FileSystemDirectory()
    {
    }

    public FileSystemDirectory(IDirectoryInfo directory)
        : base(directory)
    {
    }
}