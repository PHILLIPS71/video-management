﻿using Giantnodes.Service.Dashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations
{
    public class FileSystemDirectoryConfiguration : IEntityTypeConfiguration<FileSystemDirectory>
    {
        public void Configure(EntityTypeBuilder<FileSystemDirectory> builder)
        {
            builder
                .HasIndex(p => p.FullPath)
                .IsUnique();

            builder
                .HasMany(p => p.Nodes)
                .WithOne(p => p.ParentDirectory)
                .HasForeignKey(p => p.ParentDirectoryId);
        }
    }
}
