using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations;

public class TranscodeConfiguration : IEntityTypeConfiguration<Transcode>
{
    public void Configure(EntityTypeBuilder<Transcode> builder)
    {
        builder
            .Property(p => p.ConcurrencyToken)
            .IsRowVersion();

        builder
            .Property(p => p.Status)
            .HasConversion<string>();
    }
}