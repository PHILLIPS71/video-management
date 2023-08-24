using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Factories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Factories;

public sealed class FileSystemEntryFactory : IFactory<FileSystemEntry>
{
    public static FileSystemEntry Build(IFileSystemService service, FileSystemDirectory parent, IFileSystemInfo info)
    {
        return info switch
        {
            IDirectoryInfo directory => new FileSystemDirectory(service, parent, directory),
            IFileInfo file => new FileSystemFile(parent, file),
            _ => throw new ArgumentOutOfRangeException(nameof(info), info, null)
        };
    }
}