using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

[ObjectType<SubtitleStream>]
public static partial class SubtitleStreamType
{
    static partial void Configure(IObjectTypeDescriptor<SubtitleStream> descriptor)
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