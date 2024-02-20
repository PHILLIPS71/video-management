using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Encoder.Persistence.Sagas;

public class EncodeOperationSagaStateConfiguration : SagaClassMap<EncodeOperationSagaState>
{
    protected override void Configure(EntityTypeBuilder<EncodeOperationSagaState> builder, ModelBuilder model)
    {
        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();
        
        builder
            .HasIndex(p => p.JobId)
            .IsUnique();
    }
}