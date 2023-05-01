using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Sagas.Configurations;

public class EncodeSagaMap : SagaClassMap<EncodeSaga>
{
    protected override void Configure(EntityTypeBuilder<EncodeSaga> builder, ModelBuilder model)
    {
        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();

        builder
            .HasIndex(p => p.FullPath)
            .IsUnique();
        
        builder
            .HasIndex(p => p.JobId)
            .IsUnique();

        builder
            .Ignore(x => x.Version);
    }
}