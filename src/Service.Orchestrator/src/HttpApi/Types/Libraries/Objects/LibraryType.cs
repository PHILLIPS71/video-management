using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.HttpApi.Types.Entries.Interfaces;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Libraries.Objects;

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

    [DataLoader]
    internal static Task<Dictionary<Guid, Library>> GetLibraryByIdAsync(
        IReadOnlyList<Guid> keys,
        ApplicationDbContext database,
        CancellationToken cancellation = default)
    {
        return database
            .Libraries
            .Where(x => keys.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellation);
    }

    [NodeResolver]
    internal static Task<Library> GetLibraryByIdAsync(
        Guid id,
        ILibraryByIdDataLoader dataloader,
        CancellationToken cancellation)
    {
        return dataloader.LoadAsync(id, cancellation);
    }

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    internal static IQueryable<Encode> Encodes([Parent] Library library, ApplicationDbContext database)
        => database.Encodes.AsNoTracking().Where(x => x.File.Library.Id == library.Id);
}