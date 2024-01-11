using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Encoder.Persistence.Sagas;

public class EncodeJobSagaMap : SagaClassMap<EncodeJobSaga>
{
    protected override void Configure(EntityTypeBuilder<EncodeJobSaga> builder, ModelBuilder model)
    {
        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();
        
        builder
            .HasIndex(p => p.JobId)
            .IsUnique();

        builder
            .HasIndex(p => p.OutputDirectoryPath)
            .IsUnique();
    }
}