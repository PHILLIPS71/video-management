﻿using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;
using Giantnodes.Service.Dashboard.Domain.Values;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations;

public class FileSystemFileConfiguration : IEntityTypeConfiguration<FileSystemFile>
{
    public void Configure(EntityTypeBuilder<FileSystemFile> builder)
    {
        builder
            .OwnsOne<PathInfo>(p => p.PathInfo);

        builder
            .OwnsMany<VideoStream>(p => p.VideoStreams);

        builder
            .OwnsMany<AudioStream>(p => p.AudioStreams);
        
        builder
            .OwnsMany<SubtitleStream>(p => p.SubtitleStreams);
    }
}