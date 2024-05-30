using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.HttpApi.Types.Entries.Interfaces;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Directories.Queries;

[QueryType]
public class FileSystemDirectoryFindOne
{
    [GraphQLType<ListType<FileSystemEntryType>>]
    [UseSingleOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<FileSystemDirectory> FileSystemDirectory([Service] ApplicationDbContext database)
    {
        return database.FileSystemDirectories.AsNoTracking();
    }
}