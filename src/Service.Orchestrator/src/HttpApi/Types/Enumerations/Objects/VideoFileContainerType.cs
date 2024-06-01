using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Enumerations.Objects;

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
    internal static VideoFileContainer? GetVideoFileContainerById(int id)
    {
        return Enumeration.TryParseByValueOrName<VideoFileContainer>(id.ToString());
    }
}