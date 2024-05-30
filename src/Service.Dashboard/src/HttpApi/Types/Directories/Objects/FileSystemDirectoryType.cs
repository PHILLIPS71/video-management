using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.HttpApi.Types.Entries.Interfaces;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Directories.Objects;

[ObjectType<FileSystemDirectory>]
public static partial class FileSystemDirectoryType
{
    static partial void Configure(IObjectTypeDescriptor<FileSystemDirectory> descriptor)
    {
        descriptor.Implements<FileSystemEntryType>();

        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(p => p.Size);

        descriptor
            .Field(p => p.PathInfo);

        descriptor
            .Field(p => p.Library);

        descriptor
            .Field(p => p.ParentDirectory);

        descriptor
            .Field(p => p.ScannedAt);

        descriptor
            .Field(p => p.Entries)
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();
    }

    [NodeResolver]
    internal static Task<FileSystemDirectory?> GetFileSystemDirectoryById(
        Guid id,
        ApplicationDbContext database,
        CancellationToken cancellation)
        => database.FileSystemDirectories.SingleOrDefaultAsync(x => x.Id == id, cancellation);
}