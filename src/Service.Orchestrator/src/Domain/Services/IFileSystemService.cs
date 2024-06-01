using System.Collections.ObjectModel;
using System.IO.Abstractions;
using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Orchestrator.Domain.Services;

public interface IFileSystemService : IApplicationService, ISingletonDependency
{
    /// <summary>
    /// Asynchronously checks whether the specified path exists using a timeout of 30 seconds.
    /// </summary>
    /// <param name="path">The path to check for existence.</param>
    /// <returns>
    /// A task representing the existence check. The task result indicates whether the path exists (true) or not (false).
    /// </returns>
    public Task<bool> Exists(string path);

    /// <summary>
    /// Retrieves a read-only collection of file system entries (files and directories) in the specified path.
    /// </summary>
    /// <param name="path">The path to retrieve file system entries from.</param>
    /// <param name="search">Specifies whether to include only the current directory or all sub-directories</param>
    /// <returns>
    /// A read-only collection of <see cref="IFileSystemInfo"/> representing the file system entries in the specified path.
    /// </returns>
    public ReadOnlyCollection<IFileSystemInfo> GetFileSystemEntries(string path, SearchOption search = SearchOption.TopDirectoryOnly);
}