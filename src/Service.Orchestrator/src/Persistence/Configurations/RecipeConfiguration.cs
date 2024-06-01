using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Orchestrator.Persistence.Configurations;

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
                value => Enumeration.Parse<VideoFileContainer, int>(value, item => item.Id == value));

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