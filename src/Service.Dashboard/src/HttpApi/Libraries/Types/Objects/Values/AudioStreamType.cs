using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects.Values;

public class AudioStreamType : ObjectType<AudioStream>
{
    protected override void Configure(IObjectTypeDescriptor<AudioStream> descriptor)
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

        descriptor
            .Field(p => p.Default);

        descriptor
            .Field(p => p.Forced);
    }
}