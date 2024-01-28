using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

public class EncodeType : ObjectType<Encode>
{
    protected override void Configure(IObjectTypeDescriptor<Encode> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id)
            .ResolveNode((context, id) =>
                context.Service<ApplicationDbContext>().Encodes.SingleOrDefaultAsync(x => x.Id == id));

        descriptor
            .Field(p => p.File);

        descriptor
            .Field(p => p.Status);

        descriptor
            .Field(p => p.Percent);

        descriptor
            .Field(p => p.Speed);

        descriptor
            .Field(p => p.StartedAt);

        descriptor
            .Field(p => p.CompletedAt);

        descriptor
            .Field(p => p.CreatedAt);

        descriptor
            .Field(p => p.UpdatedAt);
    }
}