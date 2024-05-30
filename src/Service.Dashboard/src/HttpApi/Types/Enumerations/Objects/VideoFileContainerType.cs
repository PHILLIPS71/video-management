using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Enumerations.Objects;

[ObjectType<VideoFileContainer>]
public static partial class VideoFileContainerType
{
    static partial void Configure(IObjectTypeDescriptor<VideoFileContainer> descriptor)
    {
        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.Extension);
    }

    [NodeResolver]
    internal static VideoFileContainer? GetById(int id)
    {
        return Enumeration.TryParseByValueOrName<VideoFileContainer>(id.ToString());
    }
}