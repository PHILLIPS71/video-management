using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Files.Queries;

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