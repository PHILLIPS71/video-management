using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Orchestrator.Persistence.Configurations;

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
                                value => Enumeration.ParseByValueOrName<VideoResolution>(value.ToString()));
                    });
            });

        builder
            .OwnsMany(p => p.AudioStreams);

        builder
            .OwnsMany(p => p.SubtitleStreams);
    }
}