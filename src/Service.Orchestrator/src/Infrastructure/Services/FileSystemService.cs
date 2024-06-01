using System.Collections.ObjectModel;
using System.IO.Abstractions;
using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;
using Giantnodes.Service.Orchestrator.Domain.Services;

namespace Giantnodes.Service.Orchestrator.Infrastructure.Services;

public class FileSystemService : IFileSystemService
{
    private readonly IFileSystem _fs;

    public FileSystemService(IFileSystem fs)
    {
        _fs = fs;
    }

    /// <inheritdoc/>
    public Task<bool> Exists(string path)
    {
        using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
        {
            return Task.Run(() => _fs.Path.Exists(path), source.Token);
        }
    }

    /// <inheritdoc/>
    public ReadOnlyCollection<IFileSystemInfo> GetFileSystemEntries(string path, SearchOption search = SearchOption.TopDirectoryOnly)
    {
        var directory = _fs.DirectoryInfo.New(path);
        if (!directory.Exists)
            return Array.Empty<IFileSystemInfo>().AsReadOnly();

        return directory
            .EnumerateFileSystemInfos("*", search)
            .Where(x => x is IDirectoryInfo || Enumeration.TryParse<VideoFileContainer>(x.Extension) != null)
            .ToList()
            .AsReadOnly();
    }
}