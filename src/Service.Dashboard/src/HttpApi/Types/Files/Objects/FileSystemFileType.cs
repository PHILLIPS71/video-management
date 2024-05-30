using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.HttpApi.Types.Entries.Interfaces;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

[ObjectType<FileSystemFile>]
public static partial class FileSystemFileType
{
    static partial void Configure(IObjectTypeDescriptor<FileSystemFile> descriptor)
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
            .Field(p => p.ProbedAt);

        descriptor
            .Field(p => p.VideoStreams)
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();

        descriptor
            .Field(p => p.AudioStreams)
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();

        descriptor
            .Field(p => p.SubtitleStreams)
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();

        descriptor
            .Field(p => p.Encodes)
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();
    }

    [NodeResolver]
    internal static Task<FileSystemFile?> GetFileSystemFileById(
        Guid id,
        ApplicationDbContext database,
        CancellationToken cancellation)
        => database.FileSystemFiles.SingleOrDefaultAsync(x => x.Id == id, cancellation);
}