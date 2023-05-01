using Giantnodes.Service.Dashboard.Domain.Entities.Encoding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations.Encoding;

public class EncodeConfiguration : IEntityTypeConfiguration<Encode>
{
    public void Configure(EntityTypeBuilder<Encode> builder)
    {
        builder
            .Property(p => p.Status)
            .HasConversion<string>();
    }
}