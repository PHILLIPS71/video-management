using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Orchestrator.HttpApi.Types.Entries.Interfaces;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Directories.Queries;

[QueryType]
internal sealed class FileSystemDirectoryFindOne
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