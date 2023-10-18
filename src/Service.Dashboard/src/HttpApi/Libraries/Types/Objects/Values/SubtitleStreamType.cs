using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects.Values;

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