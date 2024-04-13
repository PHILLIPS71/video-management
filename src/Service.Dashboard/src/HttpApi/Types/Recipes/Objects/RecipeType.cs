using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Recipes.Objects;

public class RecipeType : ObjectType<Recipe>
{
    protected override void Configure(IObjectTypeDescriptor<Recipe> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id)
            .ResolveNode((context, id) =>
                context.Service<ApplicationDbContext>().Recipes.SingleOrDefaultAsync(x => x.Id == id));

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
            .Field("is_encodable")
            .Type<BooleanType>()
            .Resolve(x => x.Parent<Recipe>().IsEncodable());

        descriptor
            .Field(p => p.CreatedAt);

        descriptor
            .Field(p => p.UpdatedAt);
    }
}