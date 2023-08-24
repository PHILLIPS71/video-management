using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Unions;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class FileSystemEntriesFindOne
{
    [GraphQLType<ListType<FileSystemEntryType>>]
    [UseSingleOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<FileSystemEntry> FileSystemEntry([Service] ApplicationDbContext database)
    {
        return database.FileSystemEntries;
    }
}