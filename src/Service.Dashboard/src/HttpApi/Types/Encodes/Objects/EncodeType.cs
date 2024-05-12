using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Encodes.Objects;

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
            .Field(p => p.Recipe);

        descriptor
            .Field(p => p.Status);

        descriptor
            .Field(p => p.Percent);

        descriptor
            .Field(p => p.Speed);

        descriptor
            .Field(p => p.Command);

        descriptor
            .Field(p => p.Output);

        descriptor
            .Field(p => p.Machine);

        descriptor
            .Field(p => p.StartedAt);

        descriptor
            .Field(p => p.FailedAt);

        descriptor
            .Field(p => p.FailureReason);

        descriptor
            .Field(p => p.CancelledAt);

        descriptor
            .Field(p => p.CompletedAt);

        descriptor
            .Field(p => p.CreatedAt);

        descriptor
            .Field(p => p.UpdatedAt);

        descriptor
            .Field(p => p.Snapshots)
            // .UsePaging()
            // .UseProjection()
            .UseFiltering()
            .UseSorting();
    }
}