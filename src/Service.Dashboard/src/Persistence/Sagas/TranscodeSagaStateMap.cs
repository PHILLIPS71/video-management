using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Sagas;

public class TranscodeSagaStateMap : SagaClassMap<TranscodeSagaState>
{
    protected override void Configure(EntityTypeBuilder<TranscodeSagaState> builder, ModelBuilder model)
    {
        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();

        builder
            .HasIndex(p => p.JobId)
            .IsUnique();

        builder
            .HasIndex(p => p.OutputFullPath)
            .IsUnique();
    }
}