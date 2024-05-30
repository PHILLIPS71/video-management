using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Encodes.Objects;

[ObjectType<EncodeSnapshot>]
public static partial class EncodeSnapshotType
{
    static partial void Configure(IObjectTypeDescriptor<EncodeSnapshot> descriptor)
    {
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

    [NodeResolver]
    internal static Task<EncodeSnapshot?> GetEncodeSnapshotById(
        Guid id,
        ApplicationDbContext database,
        CancellationToken cancellation)
        => database.EncodeSnapshots.SingleOrDefaultAsync(x => x.Id == id, cancellation);
}