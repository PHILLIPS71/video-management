using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Orchestrator.Domain.Values;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Files.Objects;

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
    }
}