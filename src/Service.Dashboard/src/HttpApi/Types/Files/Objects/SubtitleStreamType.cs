using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

public class SubtitleStreamType : ObjectType<SubtitleStream>
{
    protected override void Configure(IObjectTypeDescriptor<SubtitleStream> descriptor)
    {
        descriptor
            .Field(p => p.Index);

        descriptor
            .Field(p => p.Codec);

        descriptor
            .Field(p => p.Language);

        descriptor
            .Field(p => p.Title);
    }
}