using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder
            .HasIndex(p => p.Name)
            .IsUnique();

        builder
            .Property(p => p.Container)
            .HasConversion(
                value => value.Id,
                value => Enumeration.TryParseByValueOrName<VideoFileContainer>(value.ToString()));

        builder
            .Property(p => p.Codec)
            .HasConversion(
                value => value.Id,
                value => Enumeration.ParseByValueOrName<EncodeCodec>(value.ToString()));

        builder
            .Property(p => p.Preset)
            .HasConversion(
                value => value.Id,
                value => Enumeration.ParseByValueOrName<EncodePreset>(value.ToString()));

        builder
            .Property(p => p.Tune)
            .HasConversion(
                value => value.Id,
                value => Enumeration.TryParseByValueOrName<EncodeTune>(value.ToString()));
    }
}