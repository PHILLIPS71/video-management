using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Recipes.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class RecipeFindOne
{
    [UseFirstOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Recipe> Recipe([Service] ApplicationDbContext database)
    {
        return database.Recipes.AsNoTracking();
    }
}