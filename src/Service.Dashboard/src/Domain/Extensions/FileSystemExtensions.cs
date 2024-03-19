using System.Collections.ObjectModel;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace System.IO.Abstractions;

public static class FileSystemExtensions
{
    /// <summary>
    /// Gets all the <see cref="IFileSystemInfo"/> objects that are within the specified <paramref name="path"/>
    /// excluding any that are not either a <see cref="IDirectoryInfo"/> or a media file.
    /// </summary>
    /// <param name="fs">A abstraction of the file system to search.</param>
    /// <param name="path">A path to search for file system entries.</param>
    /// <param name="search">Specifies whether the operation should include only the current directory or all sub-directories</param>
    /// <returns>A read-only collection of the file system entries on the file system.</returns>
    public static ReadOnlyCollection<IFileSystemInfo> GetVideoFiles(
        this IFileSystem fs,
        string path,
        SearchOption search = SearchOption.TopDirectoryOnly)
    {
        var directory = fs.DirectoryInfo.New(path);
        if (!directory.Exists)
            return Array.Empty<IFileSystemInfo>().AsReadOnly();

        return directory
            .EnumerateFileSystemInfos("*", search)
            .Where(x => x is IDirectoryInfo || Giantnodes.Infrastructure.Enumeration.TryParse<VideoFileContainer>(x.Extension) != null)
            .ToList()
            .AsReadOnly();
    }
}