using Giantnodes.Service.Dashboard.Domain.Entities.Libraries;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;

namespace Giantnodes.Service.Dashboard.HttpApi.GraphQL.Libraries.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class FindManyLibraries
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Library> Libraries(ApplicationDbContext database)
    {
        return database.Libraries;
    }
}