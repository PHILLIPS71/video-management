using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations;

public class EncodeConfiguration : IEntityTypeConfiguration<Encode>
{
    public void Configure(EntityTypeBuilder<Encode> builder)
    {
        builder
            .Property(p => p.Status)
            .HasConversion<string>();

        builder
            .Property(p => p.Percent)
            .HasPrecision(3, 2);

        builder
            .OwnsOne(p => p.Speed);

        builder
            .OwnsOne(p => p.Machine, machine =>
            {
                machine
                    .Property(p => p.ProcessorType)
                    .HasConversion<string>();
            });
    }
}