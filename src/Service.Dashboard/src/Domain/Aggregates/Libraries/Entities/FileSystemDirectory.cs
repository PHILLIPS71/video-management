using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Factories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class FileSystemDirectory : FileSystemEntry
{
    private readonly List<FileSystemEntry> _entries;

    public IReadOnlyCollection<FileSystemEntry> Entries => _entries.AsReadOnly();

    protected FileSystemDirectory()
    {
        _entries = new List<FileSystemEntry>();
    }

    public FileSystemDirectory(IDirectoryInfo directory)
        : base(directory)
    {
        _entries = new List<FileSystemEntry>();
    }

    public void Scan(ILibraryService service)
    {
        var entries = service.GetFileSystemInfos(this);

        // remove any nodes that no longer exist within the directory
        _entries.RemoveAll(x => entries.All(y => y.FullName != x.PathInfo.FullName));

        foreach (var entry in entries)
        {
            var node = _entries.SingleOrDefault(x => x.PathInfo.FullName == entry.FullName);
            if (node == null)
            {
                node = FileSystemEntryFactory.Build(entry);
                _entries.Add(node);
            }

            switch (entry)
            {
                case IFileInfo file:
                    ((FileSystemFile)node).SetSize(file);
                    break;
                
                case IDirectoryInfo:
                    var directory = (FileSystemDirectory)node;
                    directory.Scan(service);
                    break;
            }
        }
    }
}