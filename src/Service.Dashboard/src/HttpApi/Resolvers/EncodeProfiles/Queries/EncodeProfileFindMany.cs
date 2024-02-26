using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.EncodeProfiles.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class EncodeProfileFindMany
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EncodeProfile> EncodeProfiles([Service] ApplicationDbContext database)
    {
        return database.EncodeProfiles.AsNoTracking();
    }
}