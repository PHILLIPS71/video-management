using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Encodes.Objects;

public class EncodeSnapshotType : ObjectType<EncodeSnapshot>
{
    protected override void Configure(IObjectTypeDescriptor<EncodeSnapshot> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id)
            .ResolveNode((context, id) =>
                context.Service<ApplicationDbContext>().EncodeSnapshots.SingleOrDefaultAsync(x => x.Id == id));

        descriptor
            .Field(p => p.Size);

        descriptor
            .Field(p => p.ProbedAt);

        descriptor
            .Field(p => p.CreatedAt);

        descriptor
            .Field(p => p.UpdatedAt);

        descriptor
            .Field(p => p.VideoStreams)
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();

        descriptor
            .Field(p => p.AudioStreams)
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();

        descriptor
            .Field(p => p.SubtitleStreams)
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();
    }
}