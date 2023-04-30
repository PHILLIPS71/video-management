using Giantnodes.Service.Dashboard.Domain.Entities.Probing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations.Probing;

public class ProbeConfiguration : IEntityTypeConfiguration<Probe>
{
    public void Configure(EntityTypeBuilder<Probe> builder)
    {
        builder
            .Property(p => p.Status)
            .HasConversion<string>();
    }
}