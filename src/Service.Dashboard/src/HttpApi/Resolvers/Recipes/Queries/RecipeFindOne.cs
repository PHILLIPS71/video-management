using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Recipes.Queries;

[QueryType]
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