using System.IO.Abstractions;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class FileSystemFile : FileSystemEntry
{
    public long Size { get; private set; }

    protected FileSystemFile()
    {
    }

    public FileSystemFile(IFileInfo file)
        : base(file)
    {
        Size = file.Length;
    }

    public void SetSize(IFileInfo file)
    {
        Size = file.Length;
    }
}