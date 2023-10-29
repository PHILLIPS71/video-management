using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

public class VideoQualityType : ObjectType<VideoQuality>
{
    protected override void Configure(IObjectTypeDescriptor<VideoQuality> descriptor)
    {
        descriptor
            .Field(p => p.Width);

        descriptor
            .Field(p => p.Height);

        descriptor
            .Field(p => p.AspectRatio);

        descriptor
            .Field(p => p.Resolution);
    }
}