using System.IO.Abstractions;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class FileSystemFile : FileSystemEntry
{
    protected FileSystemFile()
    {
    }

    public FileSystemFile(FileSystemDirectory parent, IFileInfo file)
        : base(parent, file)
    {
        Size = file.Length;
    }

    public void SetSize(IFileInfo file)
    {
        if (file.FullName != PathInfo.FullName)
            throw new AggregateException("the file info path provided does not match the file system entry");

        Size = file.Length;
    }
}