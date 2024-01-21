using System.Collections.ObjectModel;
using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

public interface IFileSystemService : IDomainService
{
    /// <summary>
    /// Checks if a path exists on the file system asynchronously.
    /// </summary>
    /// <param name="path">A path to search for file system entries.</param>
    /// <returns>A bool indicating if the path exists on the file system.</returns>
    public Task<bool> Exists(string path);

    /// <summary>
    /// Gets all the <see cref="IFileSystemInfo"/> objects that are within the specified <paramref name="path"/>
    /// excluding any that are not either a <see cref="IDirectoryInfo"/> or a media file.
    /// </summary>
    /// <param name="path">A path to search for file system entries.</param>
    /// <param name="search">Specifies whether the operation should include only the current directory or all sub-directories</param>
    /// <returns>A read-only collection of the file system entries on the file system.</returns>
    public ReadOnlyCollection<IFileSystemInfo> GetFileSystemEntries(string path, SearchOption search = SearchOption.TopDirectoryOnly);
}