using Giantnodes.Service.Dashboard.Domain.Entities.Files;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;

namespace Giantnodes.Service.Dashboard.HttpApi.GraphQL.Files.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class FindManyFileSystemNodes
{
    [UsePaging]
    // [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<FileSystemNode> FileSystemNodes(ApplicationDbContext database)
    {
        return database.Nodes;
    }
}