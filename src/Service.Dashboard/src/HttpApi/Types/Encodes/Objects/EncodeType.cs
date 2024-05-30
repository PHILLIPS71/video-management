using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Encodes.Objects;

[ObjectType<Encode>]
public static partial class EncodeType
{
    static partial void Configure(IObjectTypeDescriptor<Encode> descriptor)
    {
        descriptor
            .Field(p => p.Id);

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
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting();
    }

    [NodeResolver]
    internal static Task<Encode?> GetEncodeById(Guid id, ApplicationDbContext database, CancellationToken cancellation)
        => database.Encodes.SingleOrDefaultAsync(x => x.Id == id, cancellation);
}