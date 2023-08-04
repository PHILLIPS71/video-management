using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

public interface ILibraryService : IDomainService
{
    /// <summary>
    /// Retrieves a read-only collection of <see cref="IFileSystemInfo" /> objects that represent the media files and
    /// any sub-directories inside the specified library.
    /// </summary>
    /// <param name="library">A library that specifies the path on which to search the file system.</param>
    /// <returns>An read-only collection of the file system entries that are inside the specified library path.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown when the path is not a directory or cannot be found.</exception>
    IReadOnlyCollection<IFileSystemInfo> GetFileSystemInfos(Library library);
}