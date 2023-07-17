using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class LibraryFindOneQuery
{
    [UseFirstOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Library> Library([Service] ApplicationDbContext database)
    {
        return database.Libraries.Include(x => x.Entries).AsNoTracking();
    }
}