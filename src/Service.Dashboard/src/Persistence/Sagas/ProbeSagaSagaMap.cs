using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Sagas;

public class ProbeSagaMap : SagaClassMap<ProbeSaga>
{
    protected override void Configure(EntityTypeBuilder<ProbeSaga> builder, ModelBuilder model)
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