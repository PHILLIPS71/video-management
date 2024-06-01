using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;
using Giantnodes.Service.Orchestrator.Domain.Values;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.Persistence.Configurations;

public class FileSystemFileConfiguration : IEntityTypeConfiguration<FileSystemFile>
{
    public void Configure(EntityTypeBuilder<FileSystemFile> builder)
    {
        builder
            .OwnsOne<PathInfo>(p => p.PathInfo);

        builder
            .OwnsMany(p => p.VideoStreams, streamBuilder =>
            {
                streamBuilder
                    .OwnsOne(p => p.Quality, qualityBuilder =>
                    {
                        qualityBuilder
                            .Property(p => p.Resolution)
                            .HasConversion(
                                value => value.Id,
                                value => Enumeration.Parse<VideoResolution, int>(value, item => item.Id == value));
                    });
            });

        builder
            .OwnsMany(p => p.AudioStreams);

        builder
            .OwnsMany(p => p.SubtitleStreams);
    }
}