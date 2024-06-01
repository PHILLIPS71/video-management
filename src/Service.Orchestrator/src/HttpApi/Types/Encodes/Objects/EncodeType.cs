using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Encodes.Objects;

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

    [DataLoader]
    internal static Task<Dictionary<Guid, Encode>> GetEncodeByIdAsync(
        IReadOnlyList<Guid> keys,
        ApplicationDbContext database,
        CancellationToken cancellation = default)
    {
        return database
            .Encodes
            .Where(x => keys.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellation);
    }

    [NodeResolver]
    internal static Task<Encode> GetEncodeByIdAsync(
        Guid id,
        IEncodeByIdDataLoader dataloader,
        CancellationToken cancellation)
    {
        return dataloader.LoadAsync(id, cancellation);
    }
}