using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.HttpApi.Types.Entries.Interfaces;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Libraries.Objects;

[ObjectType<Library>]
public static partial class LibraryType
{
    static partial void Configure(IObjectTypeDescriptor<Library> descriptor)
    {
        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(p => p.Name);

        descriptor
            .Field(p => p.Slug);

        descriptor
            .Field(p => p.PathInfo);

        descriptor
            .Field(p => p.Status);

        descriptor
            .Field(p => p.IsWatched);

        descriptor
            .Field(p => p.Entries)
            .Type<ListType<FileSystemEntryType>>()
            .UseProjection()
            .UseFiltering()
            .UseSorting();
    }

    [NodeResolver]
    internal static Task<Library?> GetLibraryById(
        Guid id,
        ApplicationDbContext database,
        CancellationToken cancellation)
        => database.Libraries.SingleOrDefaultAsync(x => x.Id == id, cancellation);

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    internal static IQueryable<Encode> Encodes([Parent] Library library, ApplicationDbContext database)
        => database.Encodes.AsNoTracking().Where(x => x.File.Library.Id == library.Id);
}