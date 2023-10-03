using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class FileSystemDirectory : FileSystemEntry
{
    public IReadOnlyCollection<FileSystemEntry> Entries { get; private set; } = new List<FileSystemEntry>();

    protected FileSystemDirectory()
    {
    }

    public FileSystemDirectory(IFileSystemService service, FileSystemDirectory? parent, IDirectoryInfo directory)
        : base(parent, directory)
    {
        SetSize(service);
    }

    public void SetSize(IFileSystemService service)
    {
        Size = service
            .GetFileSystemEntries(PathInfo.FullName, SearchOption.AllDirectories)
            .OfType<IFileInfo>()
            .Sum(x => x.Length);
    }
}