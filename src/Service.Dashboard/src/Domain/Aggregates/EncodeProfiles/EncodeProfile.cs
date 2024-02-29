using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Specifications;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;

public class EncodeProfile : AggregateRoot<Guid>, ITimestampableEntity
{
    private readonly List<Encode> _encodes = new();

    public string Name { get; private set; }

    public string Slug { get; private set; }

    public string Container { get; private set; }

    public EncodeCodec Codec { get; private set; }

    public EncodePreset Preset { get; private set; }

    public EncodeTune? Tune { get; private set; }

    public int? Quality { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Encode> Encodes { get; private set; }

    private EncodeProfile()
    {
    }

    public EncodeProfile(
        string name,
        string container,
        EncodeCodec codec,
        EncodePreset preset,
        EncodeTune? tune = null,
        int? quality = null)
    {
        Name = name;
        Slug = name.ToSlug();
        Id = NewId.NextSequentialGuid();
        Container = container;
        Codec = codec;
        Preset = preset;
        Tune = tune;
        Quality = quality;
        Encodes = _encodes;
    }
    
    public bool IsEncodable()
    {
        return new IsEncodableSpecification().IsSatisfiedBy(this);
    }
}