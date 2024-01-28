using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations;

public class EncodeSnapshotConfiguration : IEntityTypeConfiguration<EncodeSnapshot>
{
    public void Configure(EntityTypeBuilder<EncodeSnapshot> builder)
    {
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