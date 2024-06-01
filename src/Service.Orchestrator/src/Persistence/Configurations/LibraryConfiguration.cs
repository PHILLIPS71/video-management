using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Domain.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Orchestrator.Persistence.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder
            .HasIndex(p => p.Name)
            .IsUnique();

        builder
            .HasIndex(p => p.Slug)
            .IsUnique();

        builder
            .Property(p => p.Status)
            .HasConversion<string>();

        builder
            .OwnsOne<PathInfo>(p => p.PathInfo);

        builder
            .HasMany(p => p.Entries)
            .WithOne(p => p.Library)
            .IsRequired();
    }
}