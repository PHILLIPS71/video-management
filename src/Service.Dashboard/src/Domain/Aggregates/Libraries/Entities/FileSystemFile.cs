using System.IO.Abstractions;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class FileSystemFile : FileSystemEntry
{
    protected FileSystemFile()
    {
    }

    public FileSystemFile( FileSystemDirectory parent, IFileInfo file)
        : base(parent, file)
    {
        Size = file.Length;
    }

    public void SetSize(IFileInfo file)
    {
        Size = file.Length;
    }
}