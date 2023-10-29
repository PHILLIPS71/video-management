using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;

public class FileSystemDirectory : FileSystemEntry
{
    public IReadOnlyCollection<FileSystemEntry> Entries { get; private set; } = new List<FileSystemEntry>();

    private FileSystemDirectory()
    {
    }

    public FileSystemDirectory(
        Library library,
        FileSystemDirectory? parent,
        IDirectoryInfo directory,
        IFileSystemService service)
        : base(library, parent, directory)
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