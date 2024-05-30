using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Recipes.Objects;

[ObjectType<Recipe>]
public static partial class RecipeType
{
    static partial void Configure(IObjectTypeDescriptor<Recipe> descriptor)
    {
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

    [NodeResolver]
    internal static Task<Recipe?> GetRecipeById(Guid id, ApplicationDbContext database, CancellationToken cancellation)
        => database.Recipes.SingleOrDefaultAsync(x => x.Id == id, cancellation);

    internal static bool IsEncodable([Parent] Recipe recipe)
        => recipe.IsEncodable();
}