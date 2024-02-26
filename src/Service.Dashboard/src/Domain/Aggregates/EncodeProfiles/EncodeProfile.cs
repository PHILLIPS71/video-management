using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;

public class EncodeProfile : AggregateRoot<Guid>, ITimestampableEntity
{
    public EncodeCodec Codec { get; private set; }

    public EncodePreset Preset { get; private set; }

    public EncodeTune Tune { get; private set; }

    public int Quality { get; private set; }

    public string Container { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public EncodeProfile()
    {
        Id = NewId.NextSequentialGuid();
    }
}