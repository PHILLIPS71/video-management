using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

public interface ILibraryService : IDomainService
{
    /// <summary>
    /// Retrieves a read-only collection of <see cref="IFileSystemInfo" /> objects that represent the media files and
    /// any sub-directories inside the specified file system directory.
    /// </summary>
    /// <param name="directory">The directory to scan in the file system.</param>
    /// <returns>An read-only collection of the file system entries on the file system.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory or cannot be found.</exception>
    IReadOnlyCollection<IFileSystemInfo> GetFileSystemInfos(FileSystemDirectory directory);
}