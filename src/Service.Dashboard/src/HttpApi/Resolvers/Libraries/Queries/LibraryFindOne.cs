using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Libraries.Queries;

[QueryType]
public class LibraryFindOne
{
    [UseFirstOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Library> Library([Service] ApplicationDbContext database)
    {
        return database.Libraries.AsNoTracking();
    }
}