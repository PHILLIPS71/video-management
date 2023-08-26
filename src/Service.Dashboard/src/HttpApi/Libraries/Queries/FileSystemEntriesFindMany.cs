using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Interfaces;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class FileSystemEntriesFindMany
{
    [GraphQLType<ListType<FileSystemEntryType>>]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<FileSystemEntry> FileSystemEntries([Service] ApplicationDbContext database)
    {
        return database.FileSystemEntries;
    }
}