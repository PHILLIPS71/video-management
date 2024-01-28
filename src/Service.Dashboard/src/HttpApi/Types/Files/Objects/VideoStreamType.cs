using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

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
            .Field(p => p.Quality);

        descriptor
            .Field(p => p.Framerate);

        descriptor
            .Field(p => p.Bitrate);

        descriptor
            .Field(p => p.PixelFormat);
    }
}