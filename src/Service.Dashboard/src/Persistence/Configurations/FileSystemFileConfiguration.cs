using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
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
            .OwnsMany<VideoStream>(p => p.VideoStreams, streamBuilder =>
            {
                streamBuilder.OwnsOne(p => p.Quality, qualityBuilder =>
                {
                    qualityBuilder
                        .Property(p => p.Resolution)
                        .HasConversion(
                            value => value.Id,
                            value => Enumeration.Parse<VideoResolution, int>(value, item => item.Id == value));
                });
            });

        builder
            .OwnsMany<AudioStream>(p => p.AudioStreams);

        builder
            .OwnsMany<SubtitleStream>(p => p.SubtitleStreams);
    }
}