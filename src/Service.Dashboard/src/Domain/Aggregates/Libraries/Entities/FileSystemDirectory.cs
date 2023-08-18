using System.IO.Abstractions;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class FileSystemDirectory : FileSystemEntry
{
    protected FileSystemDirectory()
    {
    }

    public FileSystemDirectory(FileSystemDirectory? parent, IDirectoryInfo directory)
        : base(parent, directory)
    {
    }
}