using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Sagas;

public class EncodeSagaStateMap : SagaClassMap<EncodeSagaState>
{
    protected override void Configure(EntityTypeBuilder<EncodeSagaState> builder, ModelBuilder model)
    {
        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();

        builder
            .HasIndex(p => p.InputFilePath)
            .IsUnique();

        builder
            .HasIndex(p => p.OutputFilePath)
            .IsUnique();

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