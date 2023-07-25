﻿using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder
            .HasIndex(p => p.Slug)
            .IsUnique();

        builder
            .Property(p => p.DriveStatus)
            .HasConversion<string>();

        builder
            .Property(p => p.ConcurrencyToken)
            .IsRowVersion();

        builder
            .OwnsOne<PathInfo>(p => p.PathInfo);

        builder
            .HasMany(e => e.Entries)
            .WithOne()
            .IsRequired();
    }
}