using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Recipes.Objects;

[ObjectType<Recipe>]
public static partial class RecipeType
{
    static partial void Configure(IObjectTypeDescriptor<Recipe> descriptor)
    {
        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(p => p.Name);

        descriptor
            .Field(p => p.Container);

        descriptor
            .Field(p => p.Codec);

        descriptor
            .Field(p => p.Preset);

        descriptor
            .Field(p => p.Tune);

        descriptor
            .Field(p => p.Quality);

        descriptor
            .Field(p => p.UseHardwareAcceleration);

        descriptor
            .Field(p => p.CreatedAt);

        descriptor
            .Field(p => p.UpdatedAt);
    }

    [DataLoader]
    internal static Task<Dictionary<Guid, Recipe>> GetRecipeByIdAsync(
        IReadOnlyList<Guid> keys,
        ApplicationDbContext database,
        CancellationToken cancellation = default)
    {
        return database
            .Recipes
            .Where(x => keys.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellation);
    }

    [NodeResolver]
    internal static Task<Recipe> GetRecipeByIdAsync(
        Guid id,
        IRecipeByIdDataLoader dataloader,
        CancellationToken cancellation)
    {
        return dataloader.LoadAsync(id, cancellation);
    }

    internal static bool IsEncodable([Parent] Recipe recipe)
        => recipe.IsEncodable();
}