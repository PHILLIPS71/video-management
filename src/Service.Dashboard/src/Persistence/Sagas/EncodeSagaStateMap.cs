using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Sagas;

public class EncodeSagaStateMap : SagaClassMap<EncodeSagaState>
{
    protected override void Configure(EntityTypeBuilder<EncodeSagaState> builder, ModelBuilder model)
    {
        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();

        builder
            .HasIndex(p => p.OutputFullPath)
            .IsUnique();
    }
}