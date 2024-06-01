using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Encodes.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
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