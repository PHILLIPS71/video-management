using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Values;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Encodes.Objects;

public class EncodeSpeedType : ObjectType<EncodeSpeed>
{
    protected override void Configure(IObjectTypeDescriptor<EncodeSpeed> descriptor)
    {
        descriptor
            .Field(p => p.Frames);

        descriptor
            .Field(p => p.Bitrate);

        descriptor
            .Field(p => p.Scale);
    }
}