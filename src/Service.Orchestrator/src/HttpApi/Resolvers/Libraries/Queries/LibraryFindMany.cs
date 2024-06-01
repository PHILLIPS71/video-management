using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Libraries.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class LibraryFindMany
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Library> Libraries([Service] ApplicationDbContext database)
    {
        return database.Libraries.AsNoTracking();
    }
}