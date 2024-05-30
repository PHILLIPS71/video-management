using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Encodes.Queries;

[QueryType]
internal sealed class EncodeFindOne
{
    [UseFirstOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Encode> Encode([Service] ApplicationDbContext database)
    {
        return database.Encodes.AsNoTracking();
    }
}