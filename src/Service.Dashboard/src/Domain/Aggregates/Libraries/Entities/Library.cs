using System.IO.Abstractions;
using System.Security;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Factories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class Library : AggregateRoot<Guid>
{
    private readonly List<FileSystemEntry> _entries = new List<FileSystemEntry>();

    public string Name { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public PathInfo PathInfo { get; private set; } = null!;

    public FileSystemStatus Status { get; private set; } = FileSystemStatus.Online;

    public FileSystemDirectory Directory => _entries
        .OfType<FileSystemDirectory>()
        .Single(x => x.PathInfo.FullName == PathInfo.FullName);

    public IReadOnlyCollection<FileSystemEntry> Entries => _entries.AsReadOnly();

    protected Library()
    {
    }

    public Library(IDirectoryInfo root, string name, string slug)
    {
        Name = name;
        Slug = slug;
        PathInfo = new PathInfo(root);

        _entries.Add(new FileSystemDirectory(null, root));
    }

    /// <summary>
    /// Traverses the <see cref="Directory"/> and any sub-directories within it, creating or updating any
    /// existing <see cref="Entries"/> as well as removing those that no longer exist.
    /// </summary>
    /// <param name="service">The <see cref="IFileSystemService"/> use to get file system entries.</param>
    public void Scan(IFileSystemService service)
    {
        var parent = Directory;
        var paths = new List<string> { parent.PathInfo.FullName };

        var stack = new Stack<string>();
        stack.Push(Directory.PathInfo.FullName);

        Status = FileSystemStatus.Online;

        while (stack.Count > 0)
        {
            var path = stack.Pop();
            parent = _entries.OfType<FileSystemDirectory>().Single(x => x.PathInfo.FullName == path);

            try
            {
                var infos = service.GetFileSystemEntries(path);

                foreach (var info in infos)
                {
                    var entry = _entries.SingleOrDefault(x => x.PathInfo.FullName == info.FullName);
                    if (entry == null)
                    {
                        entry = FileSystemEntryFactory.Build(parent, info);
                        _entries.Add(entry);
                    }

                    switch (info)
                    {
                        case IFileInfo file:
                            ((FileSystemFile)entry).SetSize(file);
                            break;

                        case IDirectoryInfo subdirectory:
                            stack.Push(subdirectory.FullName);
                            break;
                    }

                    paths.Add(info.FullName);
                }
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException or SecurityException or IOException)
                    Status = FileSystemStatus.Degraded;

                else if (ex is DirectoryNotFoundException)
                    Status = FileSystemStatus.Offline;

                else
                    throw;
            }
        }

        _entries.RemoveAll(x => paths.TrueForAll(path => path != x.PathInfo.FullName));
    }
}