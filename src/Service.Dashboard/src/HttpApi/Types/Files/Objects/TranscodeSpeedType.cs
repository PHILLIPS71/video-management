using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

public class TranscodeSpeedType : ObjectType<TranscodeSpeed>
{
    protected override void Configure(IObjectTypeDescriptor<TranscodeSpeed> descriptor)
    {
        descriptor
            .Field(p => p.Frames);

        descriptor
            .Field(p => p.Bitrate);

        descriptor
            .Field(p => p.Scale);
    }
}