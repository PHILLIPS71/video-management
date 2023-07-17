using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Factories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Factories;

public sealed class FileSystemEntryFactory : IFactory<FileSystemEntry>
{
    public static FileSystemEntry Build(IFileSystemInfo info)
    {
        return info switch
        {
            IDirectoryInfo directory => new FileSystemDirectory(directory),
            IFileInfo file => new FileSystemFile(file),
            _ => throw new ArgumentOutOfRangeException(nameof(info), info, null)
        };
    }
}