using System.Collections.ObjectModel;
using System.IO.Abstractions;
using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;

public class FileSystemService : IFileSystemService
{
    private readonly IFileSystem _fs;

    public FileSystemService(IFileSystem fs)
    {
        _fs = fs;
    }

    /// <inheritdoc/>
    public ReadOnlyCollection<IFileSystemInfo> GetFileSystemEntries(string path, SearchOption search = SearchOption.TopDirectoryOnly)
    {
        var directory = _fs.DirectoryInfo.New(path);
        if (!directory.Exists)
            throw new DirectoryNotFoundException();

        return directory
            .EnumerateFileSystemInfos("*", search)
            .Where(x => x is IDirectoryInfo || Enumeration.TryParse<MediaFileExtension>(x.Extension) != null)
            .ToList()
            .AsReadOnly();
    }
}