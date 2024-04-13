using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly ApplicationDbContext _database;

    public RecipeRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public IQueryable<Recipe> ToQueryable()
    {
        return _database
            .Recipes
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<Recipe, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().AnyAsync(predicate, cancellation);
    }

    public Task<Recipe> SingleAsync(
        Expression<Func<Recipe, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleAsync(predicate, cancellation);
    }

    public Task<Recipe?> SingleOrDefaultAsync(
        Expression<Func<Recipe, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<Recipe>> ToListAsync(
        Expression<Func<Recipe, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().Where(predicate).ToListAsync(cancellation);
    }

    public Recipe Create(Recipe entity)
    {
        return _database.Recipes.Add(entity).Entity;
    }

    public Recipe Delete(Recipe entity)
    {
        return _database.Recipes.Remove(entity).Entity;
    }
}