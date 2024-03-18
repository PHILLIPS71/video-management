using Ardalis.GuardClauses;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Specifications;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;

public class EncodeProfile : AggregateRoot<Guid>, ITimestampableEntity, ISoftDeletableEntity
{
    private readonly List<Encode> _encodes = new();

    public string Name { get; private set; }

    public VideoFileContainer? Container { get; private set; }

    public EncodeCodec Codec { get; private set; }

    public EncodePreset Preset { get; private set; }

    public EncodeTune? Tune { get; private set; }

    public int? Quality { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Encode> Encodes { get; private set; }

    private EncodeProfile()
    {
    }

    public EncodeProfile(
        string name,
        VideoFileContainer? container,
        EncodeCodec codec,
        EncodePreset preset,
        EncodeTune? tune = null,
        int? quality = null)
    {
        Id = NewId.NextSequentialGuid();
        Name = name;
        Container = container;
        Codec = codec;
        Preset = preset;
        Tune = tune;
        Quality = quality;
        Encodes = _encodes;
    }

    public void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name.Trim();
    }

    public void SetContainer(VideoFileContainer? container)
    {
        if (container != null)
            Guard.Against.OutOfRange(container);

        Container = container;
    }

    public void SetCodec(EncodeCodec codec)
    {
        Guard.Against.OutOfRange(codec);

        Codec = codec;
    }

    public void SetPreset(EncodePreset preset)
    {
        Guard.Against.OutOfRange(preset);

        Preset = preset;
    }

    public void SetTune(EncodeTune? tune)
    {
        if (tune != null)
        {
            Guard.Against.OutOfRange(tune);
            Guard.Against.NotFound(Codec.Tunes, tune);
        }

        Tune = tune;
    }

    public void SetQuality(int? quality)
    {
        if (quality.HasValue)
            Guard.Against.OutOfRange(quality.Value, nameof(quality), Codec.Quality.Min, Codec.Quality.Max);

        Quality = quality;
    }

    public bool IsEncodable()
    {
        return new IsEncodableSpecification().IsSatisfiedBy(this);
    }
}