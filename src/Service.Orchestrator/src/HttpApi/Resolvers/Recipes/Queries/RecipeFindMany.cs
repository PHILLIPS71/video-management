using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Recipes.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class RecipeFindMany
{
    [UsePaging]
    // [UseProjection] using projection cannot translate Recipe.Codec.Tunes preventing is_encodable working
    [UseFiltering]
    [UseSorting]
    public IQueryable<Recipe> Recipes([Service] ApplicationDbContext database)
    {
        return database.Recipes.AsNoTracking();
    }
}