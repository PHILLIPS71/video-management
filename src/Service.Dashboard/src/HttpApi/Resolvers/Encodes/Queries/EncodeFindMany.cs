using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Encodes.Queries;

[QueryType]
public class EncodeFindMany
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Encode> Encodes([Service] ApplicationDbContext database)
    {
        return database.Encodes.AsNoTracking();
    }
}