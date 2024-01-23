using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;

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
        IDirectoryInfo directory)
        : base(library, parent, directory)
    {
        SetSize(directory.FileSystem);
    }

    public void SetSize(IFileSystem fs)
    {
        Size = fs
            .GetVideoFiles(PathInfo.FullName, SearchOption.AllDirectories)
            .OfType<IFileInfo>()
            .Sum(x => x.Length);
    }
}