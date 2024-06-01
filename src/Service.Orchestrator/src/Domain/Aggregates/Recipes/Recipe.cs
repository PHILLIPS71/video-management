using Ardalis.GuardClauses;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes.Specifications;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes;

public class Recipe : AggregateRoot<Guid>, ITimestampableEntity, ISoftDeletableEntity
{
    private readonly List<Encode> _encodes = new();

    public string Name { get; private set; }

    public VideoFileContainer? Container { get; private set; }

    public EncodeCodec Codec { get; private set; }

    public EncodePreset Preset { get; private set; }

    public EncodeTune? Tune { get; private set; }

    public int? Quality { get; private set; }

    public bool UseHardwareAcceleration { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Encode> Encodes { get; private set; }

    private Recipe()
    {
    }

    public Recipe(
        string name,
        VideoFileContainer? container,
        EncodeCodec codec,
        EncodePreset preset,
        bool useHardwareAcceleration,
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
        UseHardwareAcceleration = useHardwareAcceleration;
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

    public void SetUseHardwareAcceleration(bool value)
    {
        UseHardwareAcceleration = value;
    }

    public bool IsEncodable()
    {
        return new IsEncodableSpecification().IsSatisfiedBy(this);
    }
}