using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.EncodeProfiles.Objects;

public class EncodeProfileType : ObjectType<EncodeProfile>
{
    protected override void Configure(IObjectTypeDescriptor<EncodeProfile> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id)
            .ResolveNode((context, id) =>
                context.Service<ApplicationDbContext>().EncodeProfiles.SingleOrDefaultAsync(x => x.Id == id));

        descriptor
            .Field(p => p.Name);

        descriptor
            .Field(p => p.Slug);

        descriptor
            .Field(p => p.Codec);

        descriptor
            .Field(p => p.Preset);

        descriptor
            .Field(p => p.Tune);

        descriptor
            .Field(p => p.Quality);

        descriptor
            .Field(p => p.Container);

        descriptor
            .Field(p => p.CreatedAt);

        descriptor
            .Field(p => p.UpdatedAt);
    }
}