using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects.Values;

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