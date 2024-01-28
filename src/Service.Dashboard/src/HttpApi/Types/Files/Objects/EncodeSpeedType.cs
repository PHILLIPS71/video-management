using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Values;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

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