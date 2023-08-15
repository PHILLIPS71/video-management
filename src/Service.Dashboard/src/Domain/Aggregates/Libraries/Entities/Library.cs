using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class Library : AggregateRoot<Guid>
{
    public string Name { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public DriveStatus DriveStatus { get; private set; } = DriveStatus.Offline;

    public FileSystemDirectory Directory { get; private set; } = null!;

    protected Library()
    {
    }

    public Library(IDirectoryInfo directory, string name, string slug)
    {
        Name = name;
        Slug = slug;
        Directory = new FileSystemDirectory(directory);
        DriveStatus = directory.Exists ? DriveStatus.Online : DriveStatus.Offline;
    }
}