using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Encodes.Objects;

[ObjectType<EncodeSpeed>]
public static partial class EncodeSpeedType
{
    static partial void Configure(IObjectTypeDescriptor<EncodeSpeed> descriptor)
    {
        descriptor
            .Field(p => p.Frames);

        descriptor
            .Field(p => p.Bitrate);

        descriptor
            .Field(p => p.Scale);
    }
}