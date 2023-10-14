using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects.Values;

public class VideoStreamType : ObjectType<VideoStream>
{
    protected override void Configure(IObjectTypeDescriptor<VideoStream> descriptor)
    {
        descriptor
            .Field(p => p.Index);

        descriptor
            .Field(p => p.Codec);

        descriptor
            .Field(p => p.Duration);

        descriptor
            .Field(p => p.Width);

        descriptor
            .Field(p => p.Height);

        descriptor
            .Field(p => p.Framerate);

        descriptor
            .Field(p => p.Ratio);

        descriptor
            .Field(p => p.Bitrate);

        descriptor
            .Field(p => p.PixelFormat);

        descriptor
            .Field(p => p.Rotation);

        descriptor
            .Field(p => p.Default);

        descriptor
            .Field(p => p.Forced);
    }
}