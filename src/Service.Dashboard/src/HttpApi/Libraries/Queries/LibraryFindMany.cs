using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Queries;

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