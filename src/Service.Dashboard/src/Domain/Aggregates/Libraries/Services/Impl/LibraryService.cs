using System.IO.Abstractions;
using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;

public class LibraryService : ILibraryService
{
    private readonly IFileSystem _fs;

    public LibraryService(IFileSystem fs)
    {
        _fs = fs;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<IFileSystemInfo> GetFileSystemInfos(FileSystemDirectory directory)
    {
        var root = _fs.DirectoryInfo.New(directory.PathInfo.FullName);
        if (!root.Exists)
            throw new DirectoryNotFoundException();

        // collect all the directories and media files inside the file system directory
        var entries = root
            .GetFileSystemInfos("*", SearchOption.TopDirectoryOnly)
            .Where(x => x is IDirectoryInfo || Enumeration.TryParse<MediaFileExtension>(x.Extension) != null)
            .ToList();

        return entries.AsReadOnly();
    }
}