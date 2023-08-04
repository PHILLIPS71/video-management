using System.IO.Abstractions;
using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using Giantnodes.Service.Dashboard.Domain.Shared;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;

public class LibraryService : ILibraryService
{
    private readonly IFileSystem _fs;

    public LibraryService(IFileSystem fs)
    {
        _fs = fs;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<IFileSystemInfo> GetFileSystemInfos(Library library)
    {
        var root = _fs.DirectoryInfo.New(library.PathInfo.FullName);
        if (!root.Exists)
            throw new DirectoryNotFoundException();

        // collect all the directories and media files inside the root directory
        var entries = root
            .GetFileSystemInfos("*", SearchOption.AllDirectories)
            .Where(x => x is IDirectoryInfo || Enumeration.TryParse<MediaFileExtension>(x.Extension) != null)
            .ToList();

        return entries.AsReadOnly();
    }
}