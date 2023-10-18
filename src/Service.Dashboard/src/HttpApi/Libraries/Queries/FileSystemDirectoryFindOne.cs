using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Interfaces;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
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