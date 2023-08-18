using System.Collections.ObjectModel;
using System.IO.Abstractions;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

public interface IFileSystemService
{
    /// <summary>
    /// Gets all the <see cref="IFileSystemInfo"/> objects that are within the specified <paramref name="path"/>
    /// excluding any that are not either a <see cref="IDirectoryInfo"/> or a media file.
    /// </summary>
    /// <param name="path">A path to search for file system entries.</param>
    /// <param name="search">Specifies whether the operation should include only the current directory or all sub-directories</param>
    /// <returns>A read-only collection of the file system entries on the file system.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory or cannot be found.</exception>
    public ReadOnlyCollection<IFileSystemInfo> GetFileSystemEntries(string path, SearchOption search = SearchOption.TopDirectoryOnly);
}