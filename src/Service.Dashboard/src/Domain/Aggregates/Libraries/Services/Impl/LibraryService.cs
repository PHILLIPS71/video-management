using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

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
        if (root.Exists == false)
            throw new DirectoryNotFoundException();

        var entries = root
            .GetFileSystemInfos("*", SearchOption.AllDirectories)
            .ToList();
        
        // todo: check permissions on files etc...

        return entries.AsReadOnly();
    }
}