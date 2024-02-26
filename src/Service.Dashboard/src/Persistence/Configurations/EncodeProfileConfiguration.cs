using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations;

public class EncodeProfileConfiguration : IEntityTypeConfiguration<EncodeProfile>
{
    public void Configure(EntityTypeBuilder<EncodeProfile> builder)
    {
        builder
            .Property(p => p.Codec)
            .HasConversion(
                value => value.Id,
                value => Enumeration.Parse<EncodeCodec, int>(value, item => item.Id == value));

        builder
            .Property(p => p.Preset)
            .HasConversion(
                value => value.Id,
                value => Enumeration.Parse<EncodePreset, int>(value, item => item.Id == value));

        builder
            .Property(p => p.Tune)
            .HasConversion(
                value => value.Id,
                value => Enumeration.Parse<EncodeTune, int>(value, item => item.Id == value));
    }
}