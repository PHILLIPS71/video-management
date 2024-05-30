using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

[ObjectType<AudioStream>]
public static partial class AudioStreamType
{
    static partial void Configure(IObjectTypeDescriptor<AudioStream> descriptor)
    {
        descriptor
            .Field(p => p.Index);

        descriptor
            .Field(p => p.Codec);

        descriptor
            .Field(p => p.Title);

        descriptor
            .Field(p => p.Language);

        descriptor
            .Field(p => p.Duration);

        descriptor
            .Field(p => p.Bitrate);

        descriptor
            .Field(p => p.SampleRate);

        descriptor
            .Field(p => p.Bitrate);

        descriptor
            .Field(p => p.Channels);
    }
}